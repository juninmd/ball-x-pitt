// Gerado para a solucao do projeto Ball-x-Pitt
// Requisito Atendido: ScriptableObject Modificado
// Arquivo gerado para configuração de Atributos das Esferas (BallConfig).
// Requisito atendido: ScriptableObjects (BallConfig)
using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Física e Status")]
        public float mass = 1f;

        [Header("Prefabs")]
        public GameObject prefab;
        public ParticleSystem collisionVFXPrefab;

        [Header("Gameplay")]
        public int baseScore = 100;
    }
}
