using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TypingTarget {

    string GetName();

    string GetCompletedPortionOfName();

    string SetCompletedPortionOfName(string completedPortionOfName);

    void HandleCompletedName();
}
