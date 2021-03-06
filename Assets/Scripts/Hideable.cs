﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hideable : MonoBehaviour
{
    private const float transparency = 0.001f;

    private Material material;
    private Hideable[] hideablesInChildren;
    private Hideable[] hideablesInParents;
    [HideInInspector] public bool hidden;
    [HideInInspector] private bool transparent;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {

        hideablesInChildren = GetComponentsInChildren<Hideable>();
        hideablesInParents = GetComponentsInParent<Hideable>();

        Hideable[] hideablesToHide = hideablesInChildren.Concat(hideablesInParents).ToArray();

        if (hidden && !transparent)
        {
            if (hideablesToHide.Length > 0)
            {
                foreach (Hideable hideable in hideablesToHide)
                {
                    hideable.hidden = true;
                    hideable.MakeMaterialTransparent();
                }
            }
            MakeMaterialTransparent();
        }

        if (!hidden && transparent)
        {
            if (hideablesToHide.Length > 0)
            {
                foreach (Hideable hideable in hideablesToHide)
                {
                    hideable.hidden = false;
                }
            }
            MakeMaterialOpaque();
        }
    }

    private void MakeMaterialTransparent()
    {
        material.SetFloat("_Mode", hidden ? 3 : 0);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        Color color = material.color;
        color.a = transparency;
        material.color = color;
        transparent = true;
    }

    private void MakeMaterialOpaque()
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;

        Color color = material.color;
        color.a = 1;
        material.color = color;
        transparent = false;
    }
}
