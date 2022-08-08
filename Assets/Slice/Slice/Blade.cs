using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME.Extensions;
using System.Threading;
using System;

namespace BLINDED_AM_ME
{
    public class Blade : MonoBehaviour
    {
        [SerializeField] private Knife _knife;
        [SerializeField] private Vector3 _forseRight;
        [SerializeField] private Vector3 _forseLeft;
        [SerializeField] private Vector3 _angularVelosityRightSide;
        [SerializeField] private Vector3 _angularVelosityLeftSide;
        [SerializeField] private string _nameLeftSide = "LeftSide";
        [SerializeField] private string _nameRightSide = "RightSide";
        
        private Material _capMaterial;
        private CancellationTokenSource _previousTaskCancel;

        private void OnEnable()
        {
            _knife.SlicedFood += Cuted;
        }

        private void OnDisable()
        {
            _knife.SlicedFood -= Cuted;
        }

        private void Cuted(GameObject gameObject, Material material)
        {
            Destroy(gameObject.GetComponent<Food>());
            Destroy(gameObject.GetComponent<MeshCollider>());
            _capMaterial = material;
            Cut(gameObject);
        }

        private void Cut(GameObject target, CancellationToken cancellationToken = default)
        {
            try
            {
                _previousTaskCancel?.Cancel();
                _previousTaskCancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cancellationToken = _previousTaskCancel.Token;
                cancellationToken.ThrowIfCancellationRequested();

                var leftSide = target;
                var leftMeshFilter = leftSide.GetComponent<MeshFilter>();
                var leftMeshRenderer = leftSide.GetComponent<MeshRenderer>();
                var materials = new List<Material>();
                leftMeshRenderer.GetSharedMaterials(materials);
                var capSubmeshIndex = 0;

                if (materials.Contains(_capMaterial))
                {
                    capSubmeshIndex = materials.IndexOf(_capMaterial);
                }
                else
                {
                    capSubmeshIndex = materials.Count;
                    materials.Add(_capMaterial);
                }

                var blade = new Plane(
                    leftSide.transform.InverseTransformDirection(transform.right),
                    leftSide.transform.InverseTransformPoint(transform.position));

                var mesh = leftMeshFilter.sharedMesh;

                // Cut
                var pieces = mesh.Cut(blade, capSubmeshIndex, cancellationToken);

                leftSide.name = _nameLeftSide;
                leftMeshFilter.mesh = pieces.Item1;
                leftMeshRenderer.sharedMaterials = materials.ToArray();

                var rightSide = new GameObject(_nameRightSide);
                var rightMeshFilter = rightSide.AddComponent<MeshFilter>();
                var rightMeshRenderer = rightSide.AddComponent<MeshRenderer>();

                rightSide.transform.SetPositionAndRotation(leftSide.transform.position, leftSide.transform.rotation);
                rightSide.transform.localScale = leftSide.transform.localScale;

                rightMeshFilter.mesh = pieces.Item2;
                rightMeshRenderer.sharedMaterials = materials.ToArray();

                // Physics 
                Destroy(rightSide.GetComponent<Collider>());

                // Replace
                var leftCollider = Replace(leftSide);
                leftCollider.sharedMesh = pieces.Item1;

                var rightCollider = Replace(rightSide);
                rightCollider.sharedMesh = pieces.Item2;

                // rigidbody
                AddRigidBody(rightSide,_forseRight,_angularVelosityRightSide);
                AddRigidBody(leftSide,_forseLeft,_angularVelosityLeftSide);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private MeshCollider Replace(GameObject side)
        {
            var meshCollider = side.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            return meshCollider;
        }

        private void AddRigidBody(GameObject side,Vector3 forse,Vector3 angularVelosity)
        {
            if (!side.GetComponent<Rigidbody>())
            {
                side.AddComponent<Rigidbody>().useGravity = true;
                var rigidBody = side.GetComponent<Rigidbody>();
                rigidBody.AddForce(forse, ForceMode.Impulse);
                rigidBody.angularVelocity = angularVelosity;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            var top = transform.position + transform.up * 0.5f;
            var bottom = transform.position - transform.up * 0.5f;

            Gizmos.DrawRay(top, transform.forward * 5.0f);
            Gizmos.DrawRay(transform.position, transform.forward * 5.0f);
            Gizmos.DrawRay(bottom, transform.forward * 5.0f);
            Gizmos.DrawLine(top, bottom);
        }
    }
}