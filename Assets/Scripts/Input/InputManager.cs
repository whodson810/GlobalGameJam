using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

// lol you can just ignore all of this shit, it's not doing anything

public class InputManager : MonoBehaviour
{
    int playerIndex = 0;
    List<PlayerData> playerData = new();

    bool keyboardInput = false;
    // Gets all the gamepads connected at the start and maps them to players.
    private void Start()
    {
        IReadOnlyList<Gamepad> gamepads;
        gamepads = Gamepad.all;

        playerData.Add(new());

        playerData[0].useKeyboard = true;

        if (gamepads.Count > 0)
        {
            playerData[0].gamepad = gamepads[0];
            playerData[0].useKeyboard = false;
        }
    }

    private void Update()
    {
        if (playerData[0].useKeyboard)
        {
            CheckKeyboardInput();
        }
        else
        {
            CheckGamepadInput();
        }
    }

    // Checks if certain keys on the keyboard have been pressed
    private void CheckKeyboardInput()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        CheckJump(keyboard.spaceKey.isPressed);

        Vector2 moveDirection = Vector2.zero;

        moveDirection += CheckAndReturn(keyboard.upArrowKey, Vector2.up);
        moveDirection += CheckAndReturn(keyboard.leftArrowKey, Vector2.left);
        moveDirection += CheckAndReturn(keyboard.downArrowKey, Vector2.down);
        moveDirection += CheckAndReturn(keyboard.rightArrowKey, Vector2.right);

        CheckVec2(moveDirection, 0);
    }

    private Vector2 CheckAndReturn(KeyControl key, Vector2 val)
    {
        if (key.isPressed)
            return val;

        return Vector2.zero;
    }

    // Reads the values from the Gamepad associated with the current player being checked
    private void CheckGamepadInput()
    {
        Gamepad pad = playerData[playerIndex].gamepad;

        CheckJump(pad.buttonSouth.isPressed);

        Vector2 stickVal = pad.leftStick.ReadValue();
        CheckVec2(stickVal, 0);
    }

    private void CheckVec2(Vector2 direction, int stickIndex)
    {
        if (direction != playerData[playerIndex].lastStickVals[stickIndex])
        {
            PublishVec2(stickIndex, direction);
            playerData[playerIndex].lastStickVals[stickIndex] = direction;
        }
    }

    private void CheckJump(bool isJumping)
    {
        if (isJumping != playerData[0].isJumping)
        {
            playerData[0].isJumping = isJumping;
            PublishJumping(isJumping);
        }
    }

    // Pubishes a Vector2 value to the EventBus
    private void PublishVec2(int stickIndex, Vector2 value)
    {
        EventBus.Publish(new Vec2InputEvent(value, playerIndex, stickIndex));
    }

    private void PublishJumping(bool jumping)
    {
        EventBus.Publish(new JumpEvent(jumping));
    }
}

// Container class for flags and data related to each player
public class PlayerData
{
    public bool useKeyboard = false;
    public bool isJumping = false;
    public Gamepad gamepad = null;
    public List<Vector2> lastStickVals;
    public float toggleTimer = 0;
    public float toggleMax = 1f;

    public PlayerData()
    {
        lastStickVals = new();
        lastStickVals.Add(Vector2.zero);
        lastStickVals.Add(Vector2.zero);
    }
}
