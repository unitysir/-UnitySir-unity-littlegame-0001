using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    [SerializeField] private float camY = -2.5f;
    [SerializeField] private float camZ = -6f;

    private Transform _player; //主角位置
    private float _smoothing = 3; //平滑度
    private Vector3 _offset; //偏移位置

    void Start()
    {
        //获取主角的位置
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        //位置偏移=相机位置-主角位置
        _offset = transform.position - _player.position;
    }

    void LateUpdate()
    {
        //targetPos（相机要移动的目标位置）=主角的位置+偏移
        //（使用TransformDirection方法使相机一直在主角背面）
        Vector3 targetPos = _player.position + _player.TransformDirection(_offset + new Vector3(0, camY, camZ));
        //利用差值运算移动相机到目标位置
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _smoothing);
        transform.LookAt(_player);
    }
}