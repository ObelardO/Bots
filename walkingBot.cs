using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {

	public float speed = 4;
	public float waitTime = .25f;
	public float turnSpeed = 90;

	public Transform wayPointsPath;


	void Start()
	{
		Vector3[] wayPoints = new Vector3[wayPointsPath.childCount];

		for(int i = 0; i < wayPoints.Length; i++) 
		{
			wayPoints[i] = wayPointsPath.GetChild(i).position;
			wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
		}
		
		StartCoroutine(MoveTo(wayPoints));
	}


	IEnumerator MoveTo(Vector3[] wayPoints)
	{
		transform.position = wayPoints[0];

		int targetWayPointID = 1;
		Vector3 targetWayPoint = wayPoints[targetWayPointID];
		transform.LookAt(targetWayPoint);

		while(true)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, speed * Time.deltaTime);
			
			if(transform.position == targetWayPoint)
			{
				targetWayPointID =(targetWayPointID + 1) % wayPoints.Length;
				targetWayPoint = wayPoints[targetWayPointID];
				
				yield return new WaitForSeconds(waitTime);
				yield return StartCoroutine(TurnTo(targetWayPoint));
			}
			
			yield return null;
		}
	}


	IEnumerator TurnTo(Vector3 targetWayPoint)
	{
		Vector3 dirToTarget = (targetWayPoint - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToTarget.z, dirToTarget.x) * Mathf.Rad2Deg;

		while(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			
			yield return null;
		}
		
	}
	
}