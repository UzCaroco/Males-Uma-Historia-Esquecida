using cherrydev;
using UnityEngine;

public class TesteDialogScript : MonoBehaviour
{
    [SerializeField] private DialogBehaviour dialogBehaviour;
    [SerializeField] private DialogNodeGraph dialogNodeGraph;

    private void Start()
    {
        dialogBehaviour.StartDialog(dialogNodeGraph);
    }
}
