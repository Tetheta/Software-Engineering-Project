using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class SpecialEffectsHelper : MonoBehaviour
{
	
	//Singleton
	private static SpecialEffectsHelper _instance;
	
	public static SpecialEffectsHelper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<SpecialEffectsHelper>();
				
				//Tell unity not to destroy this when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	public ParticleSystem smallExplosion;
	public ParticleSystem bigBang;
	public ParticleSystem smallBang;
	//Simply here to avoid the parent's being called
	void Start()
	{
		
	}
	void Update()
	{
		
	}
	
	void Awake()
	{
		//Register the singleton
		//if (Instance != null)
		//{
		//    Debug.Log("Multiple Instances, should be fine I hope - SpecialEffectsHelper");
		//    //Debug.LogError("Multiple instances of SpecialEffectsHelper";)
		//    return;
		//}
		
		// allow only one instance of the SpecialEffectsHelper
		//if (Instance != null && Instance != this)
		//{
		//    Destroy(gameObject);
		//    Debug.Log("Multiple Instances of SpecialEffectsHelper, destroying");
		//    return;
		//}
		
		if (_instance == null)
		{
			//If I'm the first instance, make me a Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find another reference in scene, destroy it!
			if (this != _instance)
			{
				Destroy(this.gameObject);
			}
		}
		
		//Instance = this;
		//DontDestroyOnLoad(this);
	}
	
	//Create an explosion at the given location
	
	public void Explosion(Vector3 position)
	{
		instantiate(smallExplosion, position);
	}
	
	public void Explosion(Vector3 position, Transform parent)
	{
		ParticleSystem temp = instantiate(smallExplosion, position);
		temp.transform.parent = parent;
	}
	
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
			Quaternion.identity)
			as ParticleSystem;
		
		//Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime);
		
		return newParticleSystem;
	}
}
