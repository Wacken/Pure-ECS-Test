using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

public class Bootstrap
{

    public static EntityArchetype s_basicEnemyPrototype;

    public static MeshInstanceRenderer s_enemyMesh;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void Initialize()
    {
        s_basicEnemyPrototype = World.Active.GetOrCreateManager<EntityManager>()
            .CreateArchetype(typeof(Heading2D), typeof(MoveSpeed), typeof(Position2D), typeof(TransformMatrix));
    }

    public static void loadScene()
    {
        EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();

        Entity enemy = entityManager.CreateEntity(s_basicEnemyPrototype);

        entityManager.SetComponentData(enemy, new Heading2D()
        { Value = new Unity.Mathematics.float2(0f, 1f) });
        entityManager.SetComponentData(enemy, new MoveSpeed { speed = 0.5f });
        entityManager.SetComponentData(enemy, new Position2D()
        { Value = new Unity.Mathematics.float2(0f, 0f) });

        entityManager.AddSharedComponentData(enemy, s_enemyMesh);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void initializeAfterScene()
    {
        s_enemyMesh = getLookFromPrototype("enemyPrototype");
    }

    private static MeshInstanceRenderer getLookFromPrototype(string protoName)
    {
        var proto = GameObject.Find(protoName);
        var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(proto);
        return result;
    }
}
