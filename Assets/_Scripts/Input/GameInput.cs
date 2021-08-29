using UnityEngine;

public class GameInput : MonoBehaviour
{
    static public GameInput instance { get; private set; }

    public GameplayInputData gameplayInput;

    private void Awake()
    {
        instance = Singleton.Setup(this, instance) as GameInput;
    }

    private void Update()
    {
        gameplayInput.Movement = 0;
        if (Input.GetKey(KeyCode.RightArrow))
            gameplayInput.Movement += 1;
        if (Input.GetKey(KeyCode.LeftArrow))
            gameplayInput.Movement -= 1;

        gameplayInput.MovementDown = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            gameplayInput.MovementDown += 1;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            gameplayInput.MovementDown -= 1;

        gameplayInput.Rotate = Input.GetKeyDown(KeyCode.UpArrow);
        gameplayInput.Fall = Input.GetKey(KeyCode.DownArrow);
        gameplayInput.Skip = Input.GetKeyDown(KeyCode.Space);
    }

    void OnDisable()
    {
        gameplayInput.Reset();
    }
}
