using System.Collections;
using UnityEngine;

public class VSS : MonoBehaviour
{
    public Animator anim;
    public int animNum = 0;

    public GameObject smoke;

    public int randF;
    public int VSSfire;

    public int maxAmmo = 20;
    public int currentAmmo;

    public float RPM = 900;

    public bool reloading = false;

    public static class AN
    {
        public const int FireII = -2;
        public const int FireI = -1;
        public const int Idle = 0;
        public const int Reload = 1;
        public const int TacReload = 2;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            StartCoroutine(ReloadCort());
        }

        if (Input.GetMouseButtonDown(0) && animNum == AN.Idle && !reloading)
        {
            StartCoroutine(FireCoroutine());
        }

        if (Input.GetMouseButtonUp(0) && (animNum == AN.FireI || animNum == AN.FireII))
        {
            animNum = AN.Idle;
            anim.SetInteger("vss", animNum);
        }
    }


    IEnumerator ReloadCort()
    {
        reloading = true;
        animNum = currentAmmo > 0 ? AN.TacReload : AN.Reload;
        anim.SetInteger("vss", animNum);

        if (animNum == AN.TacReload)
        {
            yield return new WaitForSeconds(3.5f);
        }
        else
        {
            yield return new WaitForSeconds(4.5f);
        }
        reloading = false;
    }

    IEnumerator FireCoroutine()
    {
        while (Input.GetMouseButton(0) && currentAmmo > 0 && !reloading)
        {
            if (animNum == AN.Idle)
            {
                animNum = Random.Range(AN.FireII, AN.Idle);
                anim.SetInteger("vss", animNum);
            }

            Fire();

            yield return new WaitForSeconds(60f / RPM); 
        }

        if (currentAmmo <= 0)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.NoAmmo);
        }

        animNum = AN.Idle;
        anim.SetInteger("vss", animNum);
    }

    public float recoil_y;
    public Vector3 recoil_direction = Vector3.forward; // 회전 축 (Z축 기준)
    public float maxRecoil = 30f; // 최대 회전 각도
    public float recoilRecovery = 2f; // 원래 위치로 돌아오는 속도

    private float currentRecoil = 0f; // 누적된 회전 각도
    private bool isRecovery = false; // 현재 돌아오는 중인지 여부
    public float recoilReturnSpeed = 1f; // 돌아오는 속도 조절 (별도의 속도)

    public void Recoil()
    {

        // 회전 각도 누적 및 제한
        float newRotation = currentRecoil + recoil_y;
        if (Mathf.Abs(newRotation) > maxRecoil)
        {
            Debug.Log("Max rotation angle reached.");
            return; // 최대 회전 각도를 초과하면 회전하지 않음
        }

        currentRecoil = newRotation;
        transform.Rotate(recoil_direction, recoil_y);
        Debug.Log("Current Rotation: " + currentRecoil);

        // 돌아오는 코루틴 실행
        StartCoroutine(ReturnToZero());
    }

    private IEnumerator ReturnToZero()
    {
        isRecovery = true;

        // 절반의 속도로 0도로 돌아가기
        while (Mathf.Abs(currentRecoil) > 0.01f)
        {
            float step = recoilReturnSpeed * Time.deltaTime * Mathf.Sign(-currentRecoil);
            currentRecoil += step;

            // 각도를 기준으로 Transform 회전 조정
            transform.rotation = Quaternion.Euler(recoil_direction * currentRecoil);

            if (Mathf.Abs(currentRecoil) <= Mathf.Abs(step))
            {
                currentRecoil = 0f;
                transform.rotation = Quaternion.identity; // 완전히 초기화
            }

            yield return null;
        }

        isRecovery = false;
    }

    public void Fire()
    {
        if (currentAmmo > 0)
        {
            
            Recoil();
            currentAmmo--;

            AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSfire);
        }
        else
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.NoAmmo);
        }
    }

    public void ReloadEnd()
    {
        if (animNum == AN.TacReload)
            currentAmmo = maxAmmo + 1; // 탄창 + 1 발
        else
            currentAmmo = maxAmmo; // 일반 재장전

        
        animNum = AN.Idle;
        anim.SetInteger("vss", animNum);
    }

    // 애니메이션 이벤트에서 호출될 함수들
    public void OutMag() => AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSoutMag);
    public void Vest() => AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSmagFromVest);
    public void Comb() => AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSmagComb);
    public void Bolt() => AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSvolt1);
    public void Bolt2() => AudioManager.instance.PlaySfx(AudioManager.Sfx.VSSvolt2);
}
