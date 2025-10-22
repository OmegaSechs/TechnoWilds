using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoTipo", menuName = "Biomon/Tipo Elementar")]
public class ElementType : ScriptableObject
{
    public string typeName;
    
    [Header("Relações Elementais")]
    public List<ElementType> weaknesses;
    public List<ElementType> resistances;
}