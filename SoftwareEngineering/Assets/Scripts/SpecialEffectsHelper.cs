using UnityEngine;

/// <summary>
/// This class is used in Travis's game, and it basically instantiates a specialEffect when called.
/// It is a singleton, so it is not destroyed upon loading different scenes and its methods can be
/// called by calling SpecialEffectsHelper.Instance.method()
/// </summary>


public class SpecialEffectsHelper : MonoBehaviour
{
	
	//Singleton
	private static SpecialEffectsHelper _instance;

	//This is a more secure way to go about what I did already in GameManager. Personally I haven't had any issues with GameManager though
	public static SpecialEffectsHelper Instance
	{
		get	//This is called when Instance is used, having this means that even if SpecialEffectsHelper doesn't have an instance somehow it'll make one
		{
			if (_instance == null)	//If we don't have an instance yet
			{
				_instance = GameObject.FindObjectOfType<SpecialEffectsHelper>(); //Find an object we can set as an instance (i.e. this object)
				
				//Tell unity not to destroy this when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	//These three particleSystems are public so we can assign them in the unity editor.
	public ParticleSystem smallExplosion;
	public ParticleSystem bigBang;
	public ParticleSystem smallBang;

	//Not used at the moment
	void Start()
	{
		
	}
	void Update()
	{
		
	}
	
	void Awake()
	{		
		if (_instance == null)
		{
			//If I'm the first instance, make me a Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find another reference in scene, destroy this GameObject!
			if (this != _instance)
			{
				Destroy(this.gameObject);
			}
		}
	}
	
	//Create an explosion at the given location
	
	public void Explosion(Vector3 position)
	{
		instantiate(smallExplosion, position);
	}

	//Overloaded in case I want it to have a parent (scrolling background parent or something)
	public void Explosion(Vector3 position, Transform parent)
	{
		ParticleSystem temp = instantiate(smallExplosion, position); //We're creating a temp variable so we can assign it a parent
		temp.transform.parent = parent;
	}

	//Same as above
	public void eggExplosion(Vector3 position, Transform parent)
	{
		ParticleSystem temp = instantiate(smallBang, position);
		temp.transform.parent = parent;
	}
	
	//overload
	public void eggExplosion(Vector3 position)
	{
		instantiate(smallBang, position);
		
	}

	//Same as above
	public void bigExplosion(Vector3 position, Transform parent)
	{
		ParticleSystem temp = instantiate(bigBang, position);
		temp.transform.parent = parent;
	}
	
	//overload
	public void bigExplosion(Vector3 position)
	{
		instantiate(bigBang, position);
		
	}
	
	//Instantiate a Particle system from prefab
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity)	//This is the default rotation orientation (Facing towards our 2D camera). 
			as ParticleSystem;	
		
		//Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime);
		
		return newParticleSystem;
	}
}
