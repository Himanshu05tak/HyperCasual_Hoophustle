using UnityEngine;

[CreateAssetMenu(
    fileName = "PhysicsProfile",
    menuName = "Basketball/Physics Profile"
)]
public class PhysicsProfileSO : ScriptableObject
{
    [Header("Profile")]
    [SerializeField] private string profileName;

    [Header("Friction")]
    [Range(0f, 1f)]
    [SerializeField] private float staticFriction = 0.55f;

    [Range(0f, 1f)]
    [SerializeField] private float dynamicFriction = 0.40f;

    [SerializeField] private PhysicsMaterialCombine frictionCombine =
        PhysicsMaterialCombine.Average;

    [Header("Bounce")]
    [Range(0f, 1f)]
    [SerializeField] private float bounciness = 0.60f;

    [SerializeField] private PhysicsMaterialCombine bounceCombine =
        PhysicsMaterialCombine.Average;

    public string ProfileName => profileName;
    public float StaticFriction => staticFriction;
    public float DynamicFriction => dynamicFriction;
    public float Bounciness => bounciness;
    public PhysicsMaterialCombine FrictionCombine => frictionCombine;
    public PhysicsMaterialCombine BounceCombine => bounceCombine;
}