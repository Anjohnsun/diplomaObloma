using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TileGenerator : MonoBehaviour
{
    private int _width;
    private GameObject _tilePrefab;

    public void Construct(int width, GameObject tilePrefab)
    {
        _width = width;
        _tilePrefab = tilePrefab;
    }

    //think about where to set delays
    public void CreateStartLines(int height)
    {
        StartCoroutine(GenerateStartLines(height));
    }

    public void CreateLine(int height)
    {
        StartCoroutine(GenerateLine(height));
    }

    private IEnumerator GenerateStartLines(int height)
    {
        if (_width % 2 == 0)
            throw new Exception("Incorrect width");

        for (int i = 0; i < height; i++)
        {
            StartCoroutine(GenerateLine(i));
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator GenerateLine(int height)
    {
        for (int i = 0; i < _width; i++)
        {
            GameObject newTile = Instantiate(_tilePrefab, new Vector3Int(i - _width / 2, height), Quaternion.identity, transform);

            newTile.transform.localScale = Vector3.zero;
            newTile.transform.DOScale(1, 0.15f);

            yield return new WaitForSeconds(0.1f + UnityEngine.Random.Range(0.1f, 0.2f));
        }

        yield return null;
    }
}
