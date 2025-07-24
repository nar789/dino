using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // ���� ������Ʈ (��: �÷��̾�)
    public float smoothSpeed = 0.125f; // ī�޶� �̵� �ӵ� ���� ����
    public Vector3 offset; // ī�޶�� ��ǥ ������Ʈ ������ �Ÿ�

    private Vector3 velocity = Vector3.zero; // SmoothDamp���� ����� �ӵ� ����

    void LateUpdate()
    {
        if (target == null)
            return;

        // ��ǥ ��ġ ���
        Vector3 desiredPosition = target.position + offset;
        // �ε巯�� �̵� ȿ�� ����
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed); // �ε巴�� �̵�
        // ī�޶� ��ġ ������Ʈ
        transform.position = smoothedPosition;

        // ī�޶� ȸ�� ���� (���ϴ� ���)
        // transform.rotation = target.rotation; // ����: ��ǥ ������Ʈ�� �����ϰ� ȸ��
    }
}