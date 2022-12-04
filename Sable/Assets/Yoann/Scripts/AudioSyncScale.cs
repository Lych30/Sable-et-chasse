using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AudioSyncScale : AudioSyncer
{
    //[SerializeField] private VisualEffect visualEffect;
    [SerializeField] private TerrainGen terrainGen;

    private void Start()
    {
        terrainGen = GetComponent<TerrainGen>();
    }

    private IEnumerator MoveToScale(float _target)
    {
        float _curr = terrainGen.depth;
        float _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Mathf.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            terrainGen.depth = _curr;

            yield return null;
        }

        m_isBeat = false;
    }

    /*private IEnumerator MoveToThickness(float _target)
    {
        float _curr = visualEffect.GetFloat("Thickness");
        float _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Mathf.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;


            visualEffect.SetFloat("Thickness", _curr);

            yield return null;
        }

        m_isBeat = false;
    }
    */

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;



        terrainGen.depth = Mathf.Lerp(terrainGen.depth, restScale, restSmoothTime * Time.deltaTime);

        //visualEffect.SetFloat("NoisFreq", Mathf.Lerp(visualEffect.GetFloat("NoisFreq"), restScale, restSmoothTime * Time.deltaTime));
        //visualEffect.SetFloat("Thickness", Mathf.Lerp(visualEffect.GetFloat("Thickness"), restThickness, restSmoothTime * Time.deltaTime));
        //transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);

        //StopCoroutine("MoveToThickness");
        //StartCoroutine("MoveToThickness", beatThickness);
    }

    public float beatScale;
    public float restScale;

    public float beatThickness;
    public float restThickness;
}
