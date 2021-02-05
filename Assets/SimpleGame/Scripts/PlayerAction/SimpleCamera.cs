using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    [SerializeField] private float camY = -2.5f;
    [SerializeField] private float camZ = -6f;

    private Transform _player;
    private float _smoothing = 3;
    private Vector3 _offset;

    void Start()
    {
        //获取主角的位置
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        //位置偏移=相机位置-主角位置
        _offset = transform.position - _player.position;
    }

    void LateUpdate()
    {
        transform.position = _player.position + _offset;
    }
}