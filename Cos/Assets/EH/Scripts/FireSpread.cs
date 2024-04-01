using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpread : MonoBehaviour
{
    public GameObject fireObject; // 생성할 불 오브젝트
    public int[] numberOfObjectsPerLevel = { 1, 3, 5, 10 }; // 각 레벨별로 생성될 불 오브젝트의 개수
    public float[] radiiPerLevel = { 1f, 3f, 5f, 10f }; // 각 레벨별로 생성될 불 오브젝트의 반지름
    public float objectLifetime = 3f; // 오브젝트 생존 시간

    // 외부에서 호출하여 불을 생성하는 메서드
    public void StartFireSpread(int level)
    {
        if (level < 0 || level >= numberOfObjectsPerLevel.Length)
        {
            Debug.LogError("Invalid level!");
            return;
        }

        SpawnFireInCircle(numberOfObjectsPerLevel[level], radiiPerLevel[level]); // 해당 레벨에 맞게 불 오브젝트 생성
    }

    void SpawnFireInCircle(int numberOfObjects, float radius)
    {
        float angleStep = 360f / numberOfObjects; // 각도 단계 계산

        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep; // 현재 각도 계산

            // 불 오브젝트가 생성될 위치 계산
            float posX = transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float posY = transform.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 spawnPosition = new Vector3(posX, posY, transform.position.z);

            GameObject obj = Instantiate(fireObject, spawnPosition, Quaternion.identity); // 불 오브젝트 생성
            StartCoroutine(DestroyAfterLifetime(obj)); // 일정 시간 후에 파괴될 수 있도록 설정
        }
    }

    IEnumerator DestroyAfterLifetime(GameObject obj)
    {
        yield return new WaitForSeconds(objectLifetime);
        Destroy(obj); // 일정 시간 후에 오브젝트 파괴
    }
}
