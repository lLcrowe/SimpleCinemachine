using UnityEngine;

namespace lLCroweTool
{
    public static class Util
    {
        /// <summary>
        /// 4개의 점으로 만드는 곡선(3차 베지어 곡선)
        /// </summary>
        /// <param name="aPoint">첫번째 위치</param>
        /// <param name="aPointHandle">첫번째 핸들</param>
        /// <param name="bPointHandle">두번째 핸들</param>
        /// <param name="bPoint">두번째 위치</param>
        /// <param name="time">0에서 1사이의 시간값</param>
        /// <returns>시간에 따른 곡선값</returns>
        public static float FourPointBezier(float aPoint, float aPointHandle, float bPointHandle, float bPoint, float time)
        {
            //B(t) = (1 - t)³ P0 + 3(1 - t)² t P1 + 3(1 - t)t² P2 + t³ P3
            //B(t) = 시간에 따른 최종값
            //P0 = aPoint
            //P1 = aPointHandle
            //P2 = bPointHandle
            //P3 = bPoint
            //t = 시간(0 <= t <= 1)

            return Mathf.Pow((1 - time), 3) * aPoint + Mathf.Pow((1 - time), 2) * 3 * time * aPointHandle
                    + Mathf.Pow(time, 2) * 3 * (1 - time) * bPointHandle + Mathf.Pow(time, 3) * bPoint;
        }


        /// <summary>
        /// 최대최소 정규화
        /// </summary>
        /// <param name="min">최소값</param>
        /// <param name="max"><최대값/param>
        /// <param name="value">현재값</param>
        /// <returns>0~1사이의 값</returns>
        public static float MinMaxNormalize(float min, float max, float value)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// 트랜스폼의 위치, 회전, 부모, 월드인지에 따라 설정하는 함수
        /// </summary>
        /// <param name="tr">트랜스폼</param>
        /// <param name="pos">위치</param>
        /// <param name="quaternion">회전</param>
        /// <param name="parent">부모</param>
        /// <param name="isWorld">월드좌표계여부</param>
        public static void InitTrObjPrefab(this Transform tr, Vector3 pos, Quaternion quaternion, Transform parent = null, bool isWorld = true)
        {
            //프리팹을 주기적으로 초기화하기때문에 만든 함수
            //순서
            //1. 부모정의
            //2. 이동 회전
            //3. 비활성화 키기체크
            tr.SetParent(parent);//디폴트가 true
            //true인 경우 개체가 이전과 동일한 월드 공간 위치, 회전 및 크기를 유지하도록 상위 상대적 위치, 크기 및 회전이 수정
            //tr.SetParent(parent, false);//참일 경우. 이전에 있던걸 유지하고 옮김
            if (isWorld)
            {
                //tr.position = pos;
                //tr.rotation = quaternion;
                tr.SetPositionAndRotation(pos, quaternion);
            }
            else
            {
                //tr.localPosition = pos;
                //tr.localRotation = quaternion;
                tr.SetLocalPositionAndRotation(pos, quaternion);
            }

            if (tr.gameObject.activeSelf == false)
            {
                tr.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 트랜스폼의 위치, 회전, 부모, 월드인지에 따라 설정하는 함수
        /// </summary>
        /// <param name="component">컴포넌트</param>
        /// <param name="pos">위치</param>
        /// <param name="quaternion">회전</param>
        /// <param name="parent">부모</param>
        /// <param name="isWorld">월드좌표계여부</param>
        public static void InitTrObjPrefab(this Component component, Vector3 pos, Quaternion quaternion, Transform parent = null, bool isWorld = true)
        {
            InitTrObjPrefab(component.transform, pos, quaternion, parent, isWorld);
        }

        /// <summary>
        /// 트랜스폼의 위치, 회전, 부모, 월드인지에 따라 설정하는 함수
        /// </summary>
        /// <param name="tr">트랜스폼</param>
        /// <param name="targetTr">변경시킬 위치와 회전</param>
        /// <param name="parent">부모</param>
        public static void InitTrObjPrefab(this Component component, Transform targetTr, Transform parent = null)
        {
            InitTrObjPrefab(component.transform, targetTr.position, targetTr.rotation, parent);
        }

        /// <summary>
        /// 트랜스폼의 위치, 부모, 월드인지에 따라 설정하는 함수(회전제외)
        /// </summary>
        /// <param name="tr">트랜스폼</param>
        /// <param name="pos">위치</param>
        /// <param name="parent">부모</param>
        /// <param name="isWorld">월드좌표계여부</param>
        public static void InitTrObjPrefab(this Transform tr, Vector3 pos, Transform parent = null, bool isWorld = true)
        {
            InitTrObjPrefab(tr, pos, tr.rotation, parent, isWorld);
        }

        /// <summary>
        /// 트랜스폼의 위치, 부모, 월드인지에 따라 설정하는 함수(회전제외)
        /// </summary>
        /// <param name="component">컴포넌트</param>
        /// <param name="pos">위치</param>
        /// <param name="parent">부모</param>
        /// <param name="isWorld">월드좌표계여부</param>
        public static void InitTrObjPrefab(this Component component, Vector3 pos, Transform parent = null, bool isWorld = true)
        {
            Transform tr = component.transform;
            InitTrObjPrefab(tr, pos, tr.rotation, parent, isWorld);
        }
    }
}
