using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public Material OnMaterial;     // 啟動時的Material
    public Material OffMaterial;    // 關閉時的Material
    public bool canAttract = true;  // 管控Attractor功能
    public bool canEject = true;    // 管控Ball Eject功能
    bool state = false;             // 機關狀態 (開/關)
    float randomWaitTime;
    MeshRenderer meshRenderer;
    Attractor attractor;
    BallEject ballEject;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer> ();
        attractor = GetComponent<Attractor> ();
        ballEject = GetComponent<BallEject> ();
        meshRenderer.material = OffMaterial;
        StartCoroutine("ToggleTrigger");          // 讓關卡能夠隨機時間打開及關閉
    }

    IEnumerator ToggleTrigger()
    {
        while (true)
        {
            state = !state;     // 變換狀態
            if (state)
            {
                meshRenderer.material = OnMaterial;  // 切換成打開的Material
                if (canAttract)
                    attractor.enabled = true;        // 打開Attractor, 讓機關能夠吸引玩家
                if (canEject) 
                    ballEject.enabled = true;        // 打開BallEject, 讓機關能夠彈飛玩家(當接觸時)
                randomWaitTime = Random.Range(1.0f, 2.0f);
            }
            else
            {
                meshRenderer.material = OffMaterial;
                attractor.enabled = false;
                ballEject.enabled = false;
                randomWaitTime = Random.Range(5.0f, 7.0f);
            }
            yield return new WaitForSeconds(randomWaitTime);   // 等待一個隨機時間
        }
    }
}

