/* Sample of simple bot behavior
ObelardO */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	// Bot parameters: Movement speed,
	// waiting time before moving, turning speed to next waypoint.
	public float speed = 4;
	public float waitTime = .25f;
	public float turnSpeed = 90;

	// Object containes empty-objects - waypoints. 
	public Transform wayPointsPath;

	void Start()
	{
		// Filling vector3 array with waypoints positions
		Vector3[] wayPoints = new Vector3[wayPointsPath.childCount];

		for(int i = 0; i < wayPoints.Length; i++) 
		{
			wayPoints[i] = wayPointsPath.GetChild(i).position;
			wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
		}
		
		// Run moving routine in 'background'
		StartCoroutine(MoveTo(wayPoints));
	}

	
	// Moving
	IEnumerator MoveTo(Vector3[] wayPoints)
	{
		// Align to first waypoint in chain
		transform.position = wayPoints[0];

		// ID of next waypoint in chain
		int targetWayPointID = 1;
		
		// Get position of next waypoint
		Vector3 targetWayPoint = wayPoints[targetWayPointID];
		
		// Orient bot exacly to next waypoint
		transform.LookAt(targetWayPoint);

		while(true)
		{
			// Move bot to target waypoint
			transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);
			
			// Wnen bot reached waypoint
			if(transform.position == targetWayPoint)
			{
				// Get ID of next waypoint in chain
				targetWayPointID = (targetWayPointID + 1) % wayPoints.Length;
				
				// Get position of next waypoint
				targetWayPoint = wayPoints[targetWayPointID];
				
				// Wait before moving to next waypoint
				yield return new WaitForSeconds(waitTime);
				
				// Turning to next waypoint direction
				yield return StartCoroutine(TurnTo(targetWayPoint));
			}
			
			yield return null;
		}
	}


	// turning
	IEnumerator TurnTo(Vector3 targetWayPoint)
	{
		// Get normilized vector to target waypoint
		Vector3 dirToTarget = (targetWayPoint - transform.position).normalized;
		
		// Get angle between bot and target waypoint
		float targetAngle = 90 - Mathf.Atan2(dirToTarget.z, dirToTarget.x) * Mathf.Rad2Deg;

		while(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
		{
			// Get step-angle (rotation in frame)
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			
			// Turn bot to required angle
			transform.eulerAngles = Vector3.up * angle;
			
			yield return null;
		}
		
	}
	
}
