using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiUserKit;

public class elevator_move : MonoBehaviour
{
    private Transform elevator, railing;
    private Vector3 downPosition, upPosition;

    private Transform railingMidLeft, railingMidRight, railingLeft, railingRight;
    [SerializeField] private Transform rotLeftClosed, rotLeftOpen, rotRightClosed, rotRightOpen, rotMLeftClosed, rotMLeftDown, rotMLeftUp, rotMRightClosed, rotMRightDown, rotMRightUp;

    private enum ElevatorState {Up, Down, Moving};
    [SerializeField] private ElevatorState _state;

    [SerializeField] private float speed = 1;

    private void Awake()
    {
        elevator = this.transform;
        railing = transform.Find("Slide railing");
        railingMidLeft = railing.Find("slide_mid_left");
        railingMidRight = railing.Find("slide_mid_right");
        railingLeft = railing.Find("slide_left"); ;
        railingRight = railing.Find("slide_right");
        _state = ElevatorState.Down;
        downPosition = elevator.position;
        upPosition = downPosition + new Vector3(0, 3.3f, 0);
    }

    private void Update()
    {

    }

    public void Move()
    {
        if (_state == ElevatorState.Down)
        {
            _state = ElevatorState.Moving;
            StartCoroutine(MoveUpwards());
        }
        else if (_state == ElevatorState.Up)
        {
            _state = ElevatorState.Moving;
            StartCoroutine(MoveDownwards());
        }
    }

    private Transform FindNearestPlayer()
    {
        Transform nearestPlayer = null;
        float nearestDistance = 9999f;

        //foreach (LocalUser user in NetworkManager.Instance.ActivePlayers)
        //{
        //    float currDistance = Vector3.Distance(elevator.position, user.transform.position);
        //    if (currDistance < nearestDistance)
        //    {
        //        nearestDistance = currDistance;
        //        nearestPlayer = user;
        //    }
        //}

        List<Transform> allPlayers = new List<Transform>();
        foreach(NetworkUser user in FindObjectsOfType<NetworkUser>())
        {
            Debug.Log(user.gameObject.name);
            float currDistance = Vector3.Distance(elevator.position, user.transform.position);
            if (currDistance < nearestDistance)
            {
                nearestDistance = currDistance;
                nearestPlayer = user.transform;
            }
        }

        return nearestPlayer;
    }

    private IEnumerator MoveUpwards()
    {
        while (railingMidLeft.rotation != rotMLeftClosed.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMLeftClosed.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightClosed.rotation, speed * Time.deltaTime);
            yield return null;
        }

        Transform player = FindNearestPlayer();
        if(player != null)
            player.SetParent(elevator);

        while (Vector3.Distance(upPosition, elevator.position) > 0.01)
        {
            elevator.position = Vector3.Lerp(elevator.position, upPosition, speed * Time.deltaTime);
            yield return null;
        }

        player.parent = null;

        while (railingMidLeft.rotation != rotMLeftUp.rotation && railingLeft.rotation != rotLeftOpen.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMLeftUp.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightUp.rotation, speed * Time.deltaTime);
            railingLeft.rotation = Quaternion.Lerp(railingLeft.rotation, rotLeftOpen.rotation, speed * Time.deltaTime);
            railingRight.rotation = Quaternion.Lerp(railingRight.rotation, rotRightOpen.rotation, speed * Time.deltaTime);
            yield return null;
        }

        _state = ElevatorState.Up;
    }

    private IEnumerator MoveDownwards()
    {
        while (railingMidLeft.rotation != rotMLeftClosed.rotation && railingLeft.rotation != rotLeftClosed.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMLeftClosed.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightClosed.rotation, speed * Time.deltaTime);
            railingLeft.rotation = Quaternion.Lerp(railingLeft.rotation, rotLeftClosed.rotation, speed * Time.deltaTime);
            railingRight.rotation = Quaternion.Lerp(railingRight.rotation, rotRightClosed.rotation, speed * Time.deltaTime);
            yield return null;
        }

        Transform player = FindNearestPlayer();
        if (player != null)
            player.SetParent(elevator);

        while (Vector3.Distance(downPosition, elevator.position) > 0.01)
        {
            elevator.position = Vector3.Lerp(elevator.position, downPosition, speed * Time.deltaTime);
            yield return null;
        }

        player.parent = null;

        while (railingMidLeft.rotation != rotMLeftDown.rotation)
        {
            railingMidLeft.rotation = Quaternion.Lerp(railingMidLeft.rotation, rotMLeftDown.rotation, speed * Time.deltaTime);
            railingMidRight.rotation = Quaternion.Lerp(railingMidRight.rotation, rotMRightDown.rotation, speed * Time.deltaTime);
            yield return null;
        }

        _state = ElevatorState.Down;
    }
}
