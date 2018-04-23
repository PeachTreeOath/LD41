using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
    public enum State { None, InDeck, InPool, InHand, InLane, InDiscard, MovingToPool, MovingToHand, MovingToDiscard, Playing }
    public enum CardType { Monster, Spell }

    public State prevState = State.None;
    public State state = State.None;
    public Owner owner = Owner.None;

    public CardModel cardModel = null;
    public CardView cardView = null;
    public CardType cardType = CardType.Monster;

    public ObjectSlot currSlot = null;
    public float scaleTarget;
    public float speed = 30f;
    public float scaleDuration = 0.5f;

    public Color bannerStartColor;
    public Color bannerActiveColor;

    public string inputText;

    private void Start()
    {
        cardView = GetComponent<CardView>();

        RectTransform banner = cardView.nameText.transform.parent as RectTransform;
        Image img = banner.GetComponent<Image>();
        if(img != null)
            banner.GetComponent<Image>().color = bannerStartColor;

        float textWidth = LayoutUtility.GetPreferredWidth(cardView.nameText.rectTransform);
        Rect bannerRect = banner.rect;
        if(textWidth < bannerRect.width)
        {
            banner.sizeDelta = new Vector2(textWidth, bannerRect.height);
        }
    }

    void Update()
    {
        bool arrived = false;
        switch (state)
        {
            case State.MovingToPool:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnMovedToPool();
                break;

            case State.MovingToDiscard:
                Vector2 target;
                if(owner == Owner.Player) {
                    target = Deck.instance.discard.transform.position;
                } else {
                    target = Enemy.instance.discard.transform.position;
                }

                arrived = UpdateMoveTo(target);
                if (arrived) OnMovedToDiscard();
                UpdateSizeTo(new Vector2(0.5f, 0.5f));
                break;

            case State.MovingToHand:
                arrived = UpdateMoveTo(currSlot.transform.position);

                if (arrived) OnMovedToHand();
                UpdateSizeTo(new Vector2(1f, 1f));
                break;

            case State.Playing:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnPlayed();
                UpdateSizeTo(new Vector2(.72f, .72f));
                break;
        }
    }

    public void SetCardModel(CardModel cardModel)
    {
        this.cardModel = cardModel;
        if (cardModel.timeToCast >= 0)
        {
            cardType = CardType.Spell;
        }

        //TODO update CARD PREFAB later
        this.cardView = GetComponent<CardView>();
        this.cardView.CreateCardImage(cardModel);
    }

    public void SetOwner(Owner newOwner)
    {
        if (owner != Owner.None)
        {
            Debug.Log(String.Format("Changing the owner of card from '{0}' to '{1}'", this.owner, newOwner));
        }

        this.owner = newOwner;
    }

    public void SetInLane()
    {
        ChangeState(State.InLane);
    }

    public void SetInPool()
    {
        ChangeState(State.InPool);
        //TODO any clean-up if this happens suddenly
    }

    public void SetInDeck()
    {
        ChangeState(State.InDeck);
        Deck.instance.ConfigureCardObjectAtDeck(this);
    }

    public void MoveToPool()
    {
        if (!AssertState(State.None) || !AssertOwner(Owner.None)) return;

        var nextSlot = Pool.instance.ClaimASlot(this);
        if (nextSlot != null)
        {
            OnMovingToPool();

            currSlot = nextSlot;
            ChangeState(State.MovingToPool);
        }
        else
        {
            Debug.Log("Tried to move to pool but it was full!");
            Bail();
        }
    }

    public void MoveToHand()
    {
        if (state == State.InHand)
        {
            Debug.Log("Cannot move to hand, card is already in hand");
            return;
        }

        var nextSlot = Hand.instance.ClaimASlot(this);
        if (nextSlot != null)
        {
            switch (state)
            {
                case State.InPool:
                    //TODO initialize for moving from pool
                    break;
                case State.InDeck: break;
                //TODO initialize for moving from deck
                default:
                    //Dunno what's going on-- force initial settings
                    break;
            }

            currSlot = nextSlot;
            ChangeState(State.MovingToHand);
        }
        else
        {
            Debug.LogError("Tried to move to hand but the hand was full!");
            Bail();
        }
    }

    public void Play(int desiredIndex = -1, bool failIfNotOpen = false)
    {
        if (!AssertOwner(Owner.Player, Owner.Enemy)) return;

        ObjectSlot nextSlot = null;
        if (owner == Owner.Player)
        {
            if (!AssertState(State.InHand)) return;
            nextSlot = LaneManager.instance.ClaimPlayerSlot(gameObject);
        }
        else
        {
            nextSlot = LaneManager.instance.ClaimEnemySlot(gameObject, desiredIndex, failIfNotOpen);
        }

        if (nextSlot != null)
        {
            OnPlaying();

            currSlot = nextSlot;
            ChangeState(State.Playing);
        }
        else
        {
            OnNoRoomForCard();
        }
    }

    public void Discard()
    {
        if (!AssertState(State.MovingToPool, State.InPool, State.InLane)) return;

        if (currSlot)
        {
            currSlot.Release();
            currSlot = null;
        }

        //TODO start scaling down to discard size?

        ChangeState(State.MovingToDiscard);
    }

    private void OnMovingToPool()
    {
        transform.position = Pool.instance.origin.transform.position;
    }

    private void OnMovedToPool()
    {
        currSlot.Occupy(this.gameObject);
        ChangeState(State.InPool);
    }

    private void OnMovedToDiscard()
    {
        if (owner == Owner.Player)
        {
            Deck.instance.Discard(this);
            ChangeState(State.InDiscard);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void OnMovedToHand()
    {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject);
        ChangeState(State.InHand);
    }

    private void OnPlaying()
    {
        if (currSlot != null)
        {
            currSlot.Release();
            currSlot = null;
        }

        //TODO any inital set or tear down from being in the hand (deregister from input manager)
    }

    private void OnPlayed()
    {
        if (!AssertState(State.Playing)) return;

        if (cardModel.prototype.inLanePrefab != null)
        {
            GameObject laneObject = GameObject.Instantiate(cardModel.prototype.inLanePrefab);

            laneObject.transform.position = currSlot.transform.position;
            CardInLane card = laneObject.GetComponent<CardInLane>();
            card.SetOwner(owner);
            card.SetCardModel(cardModel);
            card.SetSlot(currSlot);
            card.cardType = cardType; //TODO should probably be in CardPrototype

            currSlot.Occupy(laneObject);

            //TODO sound effects or whatever (maybe configed on card prototype)
        }
        else
        {
            currSlot.Release();
            currSlot = null;
        }

        Destroy(gameObject);
    }

    private void OnNoRoomForCard()
    {
        //Need to clear input
        inputText = "";
        //Now reset all highlighting
        cardView.nameText.text = cardModel.name;
    }

    private void ChangeState(State state)
    {
        prevState = this.state;
        this.state = state;
    }

    private bool AssertState(params State[] expecteds)
    {
        var assert = expecteds.Any(expected => state == expected);
        if (!assert)
        {
            var expectedList = string.Join(",", Array.ConvertAll(expecteds, e => e.ToString()));
            Debug.Log(string.Format("Expected to be in state '{0}', but state was '{1}'", expectedList, state));
            state = State.None;
        }

        return assert;
    }

    private bool AssertOwner(params Owner[] expecteds)
    {
        var assert = expecteds.Any(expected => owner == expected);
        if (!assert)
        {
            var expectedList = string.Join(",", Array.ConvertAll(expecteds, e => e.ToString()));
            Debug.Log(string.Format("Expected owner to be '{0}', but was '{1}'", expectedList, owner));
            state = State.None;
        }

        return assert;
    }

    private bool UpdateMoveTo(Vector2 destination)
    {
        var step = Time.deltaTime * speed;

        var curr = new Vector2(transform.position.x, transform.position.y);
        var vec = destination - curr;
        var dist = vec.magnitude;

        if (step > dist)
        {
            transform.position = destination;
            return true;
        }
        else
        {
            vec = vec.normalized * step;
            transform.position = transform.position + new Vector3(vec.x, vec.y, 0);

            return false;
        }
    }

    private bool UpdateSizeTo(Vector2 destinationSize)
    {
        var step = Time.deltaTime * speed;

        var curr = new Vector2(transform.localScale.x, transform.localScale.y);
        var vec = destinationSize - curr;
        var dist = vec.magnitude;

        if (step > dist)
        {
            transform.localScale = destinationSize;
            return true;
        }
        else
        {
            vec = vec.normalized * step;
            transform.localScale = transform.localScale + new Vector3(vec.x, vec.y, 0);

            return false;
        }
    }

    private void Bail()
    {
        ChangeState(State.None);
        //TODO in release version, just destroy?
    }

    public void MatchedWord(Owner actor = Owner.Player)
    {
        //Can be in either Hand or Pool when a match happens
        if (state == State.InHand)
        {
            if (owner == Owner.Player)
            {
                Play();
            }
        }
        else if (state == State.InPool)
        {
            SetOwner(actor);
            Discard();
        }
    }

    public void InputText(string inputString)
    {
        foreach (char c in inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (inputText.Length != 0)
                {
                    inputText = inputText.Substring(0, inputText.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                //enter clears what they typed
                inputText = "";
            }
            else
            {
                inputText += c;
            }
        }

        string cardName = cardModel.name;
        string shortCardName = cardModel.name;
        TextMeshProUGUI cardTextField = cardView.nameText;

        if (cardName.Equals(inputText, StringComparison.CurrentCultureIgnoreCase))
        {
            MatchedWord();
        }
        else if (inputText.Length > 0 && cardName.StartsWith(inputText, StringComparison.CurrentCultureIgnoreCase))
        {
            shortCardName = cardModel.name;

            string highlightedLetters = cardName.Substring(0, inputText.Length);
            string restOfWord = cardName.Substring(inputText.Length);
            cardTextField.text = "<color=blue>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";

            float textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
            float bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;

            cardTextField.transform.parent.GetComponent<Image>().color = bannerActiveColor;

            int frontNumToRemove = 0;
            int backNumToRemove = 0;

            //This makes it so we always show 10 highlighted characters then the rest are not highlighted
            while(textWidth > bannerWidth && shortCardName.Substring(0, inputText.Length - frontNumToRemove).Length > 10)
            {
                //Remove a character from the front
                frontNumToRemove++;
                shortCardName = shortCardName.Substring(1);

                //Recalculate size
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "..." + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }

            //Remove more from the back till it fits
            while(textWidth > bannerWidth)
            {
                //Remove a character from the back
                backNumToRemove++;
                shortCardName = shortCardName.Substring(0, shortCardName.Length - 1);

                //Recalculate size
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "..." + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }

            //If there are no more to remove
            if (backNumToRemove == 0)
            {
                //Get rid of the ...
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }



        }
        else if (inputText.Length > 0)
        {
            //User has typed something but it doesn't match anything, we need to force a backspace
            inputText = inputText.Substring(0, inputText.Length - 1);
        }
        else
        {
            float textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
            float bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            while(textWidth > bannerWidth){
                shortCardName = shortCardName.Substring(0, shortCardName.Length - 1);
                cardTextField.text = shortCardName + "...";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }

            RectTransform banner = cardView.nameText.transform.parent as RectTransform;
            Image img = banner.GetComponent<Image>();
            if(img != null)
                banner.GetComponent<Image>().color = bannerStartColor;
        }

    }
}

