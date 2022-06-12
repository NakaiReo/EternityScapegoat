using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarController : MonoBehaviour
{
	GameObject Player;
	Rigidbody2D rb;

	public Transform targetPosition;
	private Seeker seeker;
	private CharacterController controller;
	public Path path;
	public float speed = 2;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	public bool reachedEndOfPath;
	public void Start()
	{
		Player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();

		seeker = GetComponent<Seeker>();
		seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
	}
	public void OnPathComplete(Path p)
	{
		Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		}
	}
	public void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		if (path == null)
		{
			return;
		}
		reachedEndOfPath = false;
		float distanceToWaypoint;
		while (true)
		{
			distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
			if (distanceToWaypoint < nextWaypointDistance)
			{
				if (currentWaypoint + 1 < path.vectorPath.Count)
				{
					currentWaypoint++;
				}
				else
				{
					reachedEndOfPath = true;
					seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
					break;
				}
			}
			else
			{
				break;
			}
		}
		var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
		Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
		Vector3 velocity = dir * speed * speedFactor;
		rb.velocity = velocity * speed;
	}

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}
}