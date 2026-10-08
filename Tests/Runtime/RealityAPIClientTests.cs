using System;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace zeroxoneafour.RealityAPIUnityClient.Tests
{
    public class RealityAPIClientTests
    {
        private RealityAPIClient _realityApiClient;
        [SetUp]
        public async Task Init()
        {
            _realityApiClient = new RealityAPIClient("ws://127.0.0.1:65432");
            await _realityApiClient.Init();
        }

        [Test]
        public async Task GetGlobalPosition()
        {
            var resVector = new Vector3(0.1f, 0.1f, 2.0f);
            Assert.That(await _realityApiClient.RequestGlobalPosition(Vector3.zero), Is.EqualTo(resVector));
        }

        [Test]
        public async Task SendHeartbeats()
        {
            for (int i = 0; i < 10; i += 1)
            {
                await Task.Delay(100);
                await _realityApiClient.SendHeartbeat();
            }
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _realityApiClient.CleanUp();
        }
    }
}
