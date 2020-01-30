using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BubbleFX : MonoBehaviour
{
    // ** PARTICLE SYSTEM - MAIN **
    private ParticleSystem PS;
    private ParticleSystemRenderer PSRenderer;
    private ShapeModule ParticleShape;
    private EmissionModule ParticleEmission;
    private SubEmittersModule ParticleSubEmitters;
    private MainModule ParticleMain;
    // ** PARTICLE SYSTEM - MAIN **

    // ** PARTICLE SYSTEM - SUB **
    private ParticleSystem PSSub;
    private ParticleSystemRenderer PSSubRenderer;
    private EmissionModule ParticleSubEmission;
    private ShapeModule ParticleSubShape;
    private MainModule ParticleSubMain;
    private ColorOverLifetimeModule ParticleSubCOL;
    // ** PARTICLE SYSTEM - SUB **

    private FishObject FishObj;

    void Start()
    {
        // Grabs the fish object of the fish
        FishObj = GetComponent<FishController>().fishObject;

        // Grabs the component of the particle system
        PS = GetComponent<ParticleSystem>();
        // Have to stop particle system to make changes
        PS.Stop();

        #region ParticleSystem - Main
        PSRenderer = GetComponent<ParticleSystemRenderer>();

        // Change properties of the particle system to fit the properties of a bubble
        // being created upon the fish
        ParticleMain = PS.main;
        ParticleMain.duration = 2;
        ParticleMain.maxParticles = 3;
        ParticleMain.startSpeed = 4;
        ParticleMain.simulationSpace = ParticleSystemSimulationSpace.World;
        ParticleMain.startSize = (FishObj.Length) - 1.5f;

        // Changes the shape and the release of the particle effect based
        // location relative to the fish's current location
        ParticleShape = PS.shape;
        ParticleShape.angle = 0;
        ParticleShape.radius = 0.1f;
        ParticleShape.position = new Vector3(0, 0, 1);
        ParticleShape.rotation = new Vector3(-93, 0, 0);

        // Changes the amount of bubbles being released
        ParticleEmission = PS.emission;
        ParticleEmission.rateOverTimeMultiplier = 5f;

        // Loads the material which contains the essential shaders and colors suitable for
        // a bubble look
        PSRenderer.material = Resources.Load<Material>("Materials/BubbleMat2");
        #endregion

        // ** DEPRECATED CODE **
        //#region ParticleSystem - Sub
        //PSSub = new ParticleSystem();

        //ParticleSubMain = PSSub.main;
        //ParticleSubMain.duration = 0.5f;
        //ParticleSubMain.startLifetime = 0.6f;
        //ParticleSubMain.startSpeed = 2.5f;
        //ParticleSubMain.startSize = Random.Range(0.05f, 0.25f);
        //ParticleSubMain.startRotation = Random.Range(0, 1);
        //ParticleSubMain.scalingMode = ParticleSystemScalingMode.Shape;
        //ParticleSubMain.maxParticles = 16;
        //ParticleSubMain.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;

        //ParticleSubEmission = PSSub.emission;
        //ParticleSubEmission.rateOverTimeMultiplier = 8;

        //ParticleSubShape = PSSub.shape;
        //ParticleSubShape.shapeType = ParticleSystemShapeType.Sphere;
        //ParticleSubShape.radius = 0.01f;

        //ParticleSubCOL = PS.colorOverLifetime;
        //GradientColorKey[] GCKeys = new GradientColorKey[]
        //{
        //    new GradientColorKey() { color = new Color(0,0,0), time = 0},
        //    new GradientColorKey() { color = new Color(0,0,0,255), time = 0.5f},
        //    new GradientColorKey() { color = new Color(0,0,0), time = 1},
        //    new GradientColorKey() { color = new Color(0,0,0,255), time = 1}
        //};
        //ParticleSubCOL.color = new MinMaxGradient()
        //{
        //    gradient = new Gradient()
        //    {
        //        mode = GradientMode.Blend,
        //        colorKeys = GCKeys
        //    }
        //};
        //ParticleSubEmitters = PS.subEmitters;
        //ParticleSubEmitters.enabled = true;
        //ParticleSubEmitters.AddSubEmitter(PSSub, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);

        //PSSubRenderer = PSSub.GetComponent<ParticleSystemRenderer>();
        //PSSubRenderer.material = Resources.Load<Material>("Materials/BubbleBurstMat");
        //PSSubRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        //PSSubRenderer.receiveShadows = true;
        //#endregion
        // ** DEPRECATED CODE **

        // Plays the particle system and initalizes a pseudo-timer which differs the rate and the amount
        // of particles being showed on the fishes per 2.1 seconds
        PS.Play();
        var Init = ParticleChange();
    }

    private async Task ParticleChange()
    {
        while (true)
        {
            ParticleMain.maxParticles = Random.Range(1, 2);
            ParticleEmission.rateOverTimeMultiplier = Random.Range(5, 8);
            await Task.Delay(2100);
        }
    }

    void Update()
    {
        // Checks if the fish is dying and if it is dying, disables the particle effect
        if (FishObj.HungerTolerance <= 0)
        {
            PS.Stop();
            GetComponent<BubbleFX>().enabled = false;
        }
    }
}
