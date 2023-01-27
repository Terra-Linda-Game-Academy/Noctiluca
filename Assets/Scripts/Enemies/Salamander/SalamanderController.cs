using AI;
using Input.ConcreteInputProviders.Enemy;
using Input.Data.Enemy;
using UnityEngine;

namespace Enemies.Salamander
{
    [RequireComponent(typeof(Perceptron))]
    public class SalamanderController : MonoBehaviour
    {
        public SalamanderInputProvider providerTemplate;
        private SalamanderInputProvider _provider;

        private Perceptron _perceptron;

        void OnEnable()
        {
            _perceptron = GetComponent<Perceptron>();

            _provider = (SalamanderInputProvider) providerTemplate.Clone(_perceptron);
        }

        void FixedUpdate()
        {
            HandleInput(_provider.GetInput());
        }

        private void HandleInput(SalamanderInput inputData)
        {
            
        }
    }
}
