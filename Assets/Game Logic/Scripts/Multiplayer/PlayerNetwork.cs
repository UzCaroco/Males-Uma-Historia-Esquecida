using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    //Usado para a movimentação do player no servidor
    //Permite que apenas o dono do objeto do player tenha permissão para sua Movimentação e Rotação
    private readonly NetworkVariable<PlayerNetworkData> netState = new (writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 vel;
    private float rotVel;

    [SerializeField] private float cheapInterpolationTime = 0.1f;
    [SerializeField] private Transform modeloPlayer;

    private void Update()
    {
        //Se o player for o dono do objeto
        if (IsOwner)
        {
            netState.Value = new PlayerNetworkData
            {
                Position = transform.position,
                Rotation = modeloPlayer.transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, netState.Value.Position, ref vel, cheapInterpolationTime);

            modeloPlayer.transform.rotation = Quaternion.Euler
                (
                    0,
                    Mathf.SmoothDampAngle(modeloPlayer.transform.rotation.eulerAngles.y, netState.Value.Rotation.y, ref rotVel, cheapInterpolationTime),
                    0
                );
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float xPos, zPos;
        private short yRot;

        internal Vector3 Position
        {
            get => new Vector3(xPos, 0, zPos);
            set
            {
                xPos = value.x;
                zPos = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0, yRot, 0);
            set => yRot = (short)value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref xPos);
            serializer.SerializeValue(ref zPos);

            serializer.SerializeValue(ref yRot);
        }
    }

    
}
