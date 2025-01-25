using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyLayoutNS;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class MoveRowTest : MonoBehaviour
{
    [Button("Test Move Row")]
    private void ShiftRow()
    {
        var allHexCoordinates = GetComponentsInChildren<HexCoordinates>();
        List<OffsetCoordinates> allOffsets = allHexCoordinates.Select(hc => hc.Offset).ToList();
        var before = new List<OffsetCoordinates>(allOffsets);
        for (var index = 0; index < allOffsets.Count; index++)
        {
            var coordinates = before[index];
            if (coordinates.Row == 0)
            {
                var lastRow = before.Max(c => c.Row);
                allHexCoordinates[index].SetCoordinates(lastRow, coordinates.Column);
                allHexCoordinates[index].transform.SetAsLastSibling();
                continue;
            }

            allHexCoordinates[index].SetCoordinates(coordinates.Row - 1, coordinates.Column);
        }
    }
}
