public class Timer {
    public float target = 0;
    private float value = 0;

    public bool Update(float deltaTime) {
        if (target > 0) {
            value += deltaTime;
            if(value > target) {
                value = 0;
                return true;
            }
        } else {
            value = 0;
        }

        return false;
    }

    public void Reset() {
        value = 0;
    } 
}