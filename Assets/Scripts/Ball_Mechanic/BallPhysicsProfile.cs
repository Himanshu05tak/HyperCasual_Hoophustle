using UnityEngine;

public class BallPhysicsProfile : MonoBehaviour
{
    [SerializeField] private Collider ballCollider;
    [SerializeField] private PhysicsProfileSO[] profiles;

    private PhysicsMaterial runtimeMaterial;

    private void Awake()
    {
        runtimeMaterial = ballCollider.material;

        runtimeMaterial.frictionCombine = PhysicsMaterialCombine.Average;
        runtimeMaterial.bounceCombine = PhysicsMaterialCombine.Average;
    }

    public void ApplyRandomProfile()
    {
        PhysicsProfileSO profile =
            profiles[Random.Range(0, profiles.Length)];

        Debug.Log("Profile " + profile.name);
        ApplyProfile(profile);
    }

    private void ApplyProfile(PhysicsProfileSO profile)
    {
        runtimeMaterial.staticFriction = profile.StaticFriction;
        runtimeMaterial.dynamicFriction = profile.DynamicFriction;
        runtimeMaterial.bounciness = profile.Bounciness;

        runtimeMaterial.frictionCombine = profile.FrictionCombine;
        runtimeMaterial.bounceCombine = profile.BounceCombine;
    }
}