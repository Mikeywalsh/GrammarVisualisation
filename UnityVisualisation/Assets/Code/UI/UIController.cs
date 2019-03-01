using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TreeVisualisation.Implementations.Grammar;

public class UIController : MonoBehaviour
{
    // Singleton reference
    public static UIController Controller;

    // Tree information
    public UnityTreeController TreeController;

    // Tree info
    public TextMeshProUGUI TreeInfoText;

    // Display depth
    public Button IncrementDisplayDepthButton;
    public Button DecrementDisplayDepthButton;
    public TextMeshProUGUI DisplayDepthText;

    private string treeInfoTemplate;
    private int displayDepth;

    void Awake()
    {
        if (Controller != null)
        {
            throw new Exception("Sinigleton reference for UIController already set");
        }

        Controller = this;
        treeInfoTemplate = TreeInfoText.text;
    }

    public void InitialiseUI()
    {
        displayDepth = TreeController.CurrentTree.MaxDepth;
        UpdateDisplayDepthUI();
        UpdateTreeInfoUI();
    }

    public void IncrementDisplayDepth()
    {
        displayDepth++;
        UpdateDisplayDepthUI();
    }

    public void DecrementDisplayDepth()
    {
        displayDepth--;
        UpdateDisplayDepthUI();
    }

    private void UpdateTreeInfoUI()
    {
        var treeInfoString = string.Format(treeInfoTemplate,
            TreeController.CurrentTree.MaxDepth,
            TreeController.CurrentTree.AllNodes.Count,
            TreeController.CurrentTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.NONTERMINAL),
            TreeController.CurrentTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.TERMINAL),
            TreeController.CurrentTree.AllNodes.Count(n => n.Data.NodeType == GrammarNodeType.ERROR));

        TreeInfoText.text = treeInfoString;
    }

    private void UpdateDisplayDepthUI()
    {
        if (displayDepth == 0)
        {
            DecrementDisplayDepthButton.gameObject.SetActive(false);
        }
        else
        {
            DecrementDisplayDepthButton.gameObject.SetActive(true);
        }

        if (displayDepth == TreeController.CurrentTree.MaxDepth)
        {
            IncrementDisplayDepthButton.gameObject.SetActive(false);
        }
        else
        {
            IncrementDisplayDepthButton.gameObject.SetActive(true);
        }

        DisplayDepthText.text = "Display Depth: " + displayDepth;
    }
}
