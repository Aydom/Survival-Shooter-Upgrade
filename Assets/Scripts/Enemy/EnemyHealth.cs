using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public GameObject FloatingTextPrefab;
	public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;

	public GameObject DropLootPrefab;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }

	GameObject _dropLootTarget;

	void Start ()
	{
		_dropLootTarget = GameObject.FindGameObjectWithTag ("DropLootTracker");
	}

    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

		if (FloatingTextPrefab && currentHealth > 0) {
			ShowFloatingText ();
		}

        if(currentHealth <= 0)
        {
            Death ();
        }
    }

	void ShowFloatingText()
	{
		var go = Instantiate (FloatingTextPrefab, transform.position, Quaternion.identity, transform);
		go.GetComponent<TextMesh> ().text = currentHealth.ToString ();
	}


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();

		for (int i = 0; i < startingHealth / 10; i++) {
			var go = Instantiate (DropLootPrefab, transform.position + new Vector3(0, Random.Range(0,2)), Quaternion.identity);
			go.GetComponent<Follow> ().Target = _dropLootTarget.transform;
		}
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
