using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    //[SerializeField] private string nextLevel;
	[SerializeField] private Object nextLevel;
    [SerializeField] private float triggerRadius = 3f;
	private bool allowTransition = false;
	private bool transitioning = false;
	private Transform player;

    private void OnEnable(){
		scoreManager.onThresholdReached += UnlockTransition;
	}

	private void OnDisable(){
		scoreManager.onThresholdReached -= UnlockTransition;
	}

	private void Start(){
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj != null) player = playerObj.transform;
	}

	private void Update(){
		if (!allowTransition || transitioning || player == null){
			if(allowTransition) Debug.Log($"[LT] blocked — transitioning:{transitioning} playerNull:{player == null}");
			return;
		}
		Vector2 shipXZ = new Vector2(transform.position.x, transform.position.z);
		Vector2 playerXZ = new Vector2(player.position.x, player.position.z);
		float dist = Vector2.Distance(shipXZ, playerXZ);
		Debug.Log($"[LT] object:{gameObject.name} XZ dist:{dist:F2} radius:{triggerRadius} shipXZ:({shipXZ.x:F1},{shipXZ.y:F1})");
		if (dist <= triggerRadius){
			transitioning = true;
			Debug.Log("[LT] Boarding — loading " + nextLevel.name);
			StartCoroutine(LoadScene());
		}
	}

	public void UnlockTransition(){
    	allowTransition = true;
		Debug.Log("Level Transition Permitted. Player can now move on to the next level.");
	}

	private void OnTriggerEnter(Collider other){
    	if(allowTransition && !transitioning && other.CompareTag("Player")){
			transitioning = true;
			StartCoroutine(LoadScene());
		}
	}

	private IEnumerator LoadScene(){
		var rb = player.GetComponent<Rigidbody>();
   		// ADD THIS CHECK:
   		if (nextLevel == null)
   		{
      		Debug.LogError("Next Level Scene is not assigned in the inspector!");
      		yield break;
   		}
		if(rb != null){
			rb.isKinematic = true;
		}

		Transform shipMesh = transform.Find("ShipMesh");
		if(shipMesh != null){
			shipMesh.localScale *= 1.15f;
		}

		Time.timeScale = 1f;
		yield return new WaitForSecondsRealtime(0.5f);
		SceneManager.LoadScene(nextLevel.name);
	}
}
