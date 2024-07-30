using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteLine : MonoBehaviour
{
    [SerializeField] private LineRenderer m_lineRenderer = null; // �~��`�悷�邽�߂� LineRenderer
    [SerializeField] private float m_radius = 0;    // �~�̔��a
    [SerializeField] private float m_lineWidth = 0;    // �~�̐��̑���

    [SerializeField] private float m_duration = 0; // �X�P�[�����o�̍Đ����ԁi�b�j
    [SerializeField] private float m_from = 0; // �X�P�[�����o�̊J�n�l
    [SerializeField] private float m_to = 0; // �X�P�[�����o�̏I���l

    private float m_elapedTime;

    private void Reset()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Awake()
    {
    }

    private void Update()
    {
        InitLineRenderer();
    }

    private void InitLineRenderer()
    {
        var segments = 360;

        m_lineRenderer.startWidth = m_lineWidth;
        m_lineRenderer.endWidth = m_lineWidth;
        m_lineRenderer.positionCount = segments;
        m_lineRenderer.loop = true;
        m_lineRenderer.useWorldSpace = false; // transform.localScale ��K�p���邽��

        var points = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            var x = Mathf.Sin(rad) * m_radius;
            var y = Mathf.Cos(rad) * m_radius;
            points[i] = new Vector3(x, y, 0);
        }

        m_lineRenderer.SetPositions(points);
    }
}
