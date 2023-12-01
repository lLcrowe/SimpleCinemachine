using lLCroweTool.Cinemachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace lLCroweTool.QC.EditorOnly
{
    [CustomEditor(typeof(CustomCinemachine))]
    public class CustomCinemachineInspectorEditor : Editor
    {
        private CustomCinemachine targetCinemachine;
        private Vector2 scrollPos;

        private void OnEnable()
        {
            targetCinemachine = (CustomCinemachine)target;
        }

        public override void OnInspectorGUI()
        {
            //�����ϻ�����ó��//1�� ó�� ���ϰ� ������ ���޴°����� �۾��ҋ� �и��� ������ �߻���
            //������Ʈ��ġ������ �� �۾��� ���� �ֳ��ϸ� �� ������Ʈ�� �θ���� ��� �Űܴٴϴµ� ���۾��� �ҽ�
            //��ġ���� �����Ǵ� ������ �߻���
            targetCinemachine.transform.localScale = Vector3.one;



            if (targetCinemachine.name != targetCinemachine.cinemachineID)
            {
                targetCinemachine.name = string.IsNullOrEmpty(targetCinemachine.cinemachineID) ? "-NullCinemachineID-" : $"-CinemachineID : {targetCinemachine.cinemachineID}-";
            }             
            EditorGUILayout.Space();            

            //��ü���
            EditorGUILayout.BeginHorizontal();
            string tempCotent = targetCinemachine.IsRun() ? "Pause" : "Play";
            if (GUILayout.Button(tempCotent) && Application.isPlaying)
            {
                targetCinemachine.ActionCamera();
            }

            if (GUILayout.Button("Stop"))
            {
                targetCinemachine.Stop();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add ObjectBatchInfo"))
            {
                if (!Application.isPlaying)
                {
                    if (EditorSceneManager.GetActiveScene().isDirty == false)
                    {
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                }
                ObjectBatchInfo newBatchInfo = new GameObject().AddComponent<ObjectBatchInfo>();
                newBatchInfo.targetCinemachine = targetCinemachine;                
                newBatchInfo.InitTrObjPrefab(targetCinemachine.transform, targetCinemachine.transform);

                newBatchInfo.contentName = "EmtpyContent";
                newBatchInfo.curve = AnimationCurve.Linear(0, 0, 1, 1);
                newBatchInfo.startTime = 0;
                newBatchInfo.endTime = 3;

                ObjectBatchInfoInspectorEditor.CreateObjectBatchInfo(newBatchInfo);

                //������ ������ �װ� �������� ����
                if (targetCinemachine.cameraBatchList.Count != 0)
                {
                    //�߰����νð��� ��
                    ObjectBatchInfo prevBatchInfo = targetCinemachine.cameraBatchList[targetCinemachine.cameraBatchList.Count - 1];
                    newBatchInfo.startTime = prevBatchInfo.endTime;
                    newBatchInfo.endTime = newBatchInfo.startTime + 3;

                    newBatchInfo.startTr.position = prevBatchInfo.startTr.position;
                    newBatchInfo.endTr.position = prevBatchInfo.endTr.position;


                    //Ʈ�������������� �����ؾߵ�
                    newBatchInfo.startTr.rotation = prevBatchInfo.startTr.rotation;
                    newBatchInfo.endTr.rotation = prevBatchInfo.endTr.rotation;

                    if (newBatchInfo.movePosType == MovePosType.Bezier)
                    {
                        newBatchInfo.startHandleTr.position = prevBatchInfo.startHandleTr.position;
                        newBatchInfo.endHandleTr.position = prevBatchInfo.endTr.position;
                    }
                }
                targetCinemachine.cameraBatchList.Add(newBatchInfo);
                return;
            }

            EditorGUILayout.Space(10);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MinHeight(200));
            //�����ϱ�
            for (int i = 0; i < targetCinemachine.cameraBatchList.Count; i++)
            {
                int index = i;

                //���׳����� ���ִ°�
                var batchObject = targetCinemachine.cameraBatchList[index];
                if (batchObject == null)
                {
                    EditorGUILayout.LabelField("Emtpy");
                    if (GUILayout.Button("Remove"))
                    {
                        targetCinemachine.cameraBatchList.RemoveAt(index);
                        break;
                    }
                    continue;
                }

                //��ư
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button($"Select -{batchObject.contentName}-"))
                {
                    Selection.activeGameObject = batchObject.gameObject;
                }

                if (GUILayout.Button($"X", GUILayout.Width(30)))
                {
                    targetCinemachine.cameraBatchList.Remove(batchObject);
                    DestroyImmediate(batchObject.gameObject);
                    if (!Application.isPlaying)
                    {
                        if (EditorSceneManager.GetActiveScene().isDirty == false)
                        {
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }
                    break;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                batchObject.startTime = EditorGUILayout.FloatField("����", batchObject.startTime, GUILayout.MinWidth(30));
                batchObject.endTime = EditorGUILayout.FloatField("��", batchObject.endTime, GUILayout.MinWidth(30));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("�ں����Ѱ� ������ ������ �����ϱ�"))
            {
                EditorSceneManager.SaveScene(targetCinemachine.gameObject.scene);
            }
            base.OnInspectorGUI();
        }
      

        private void OnSceneGUI()
        {
            CustomCinemachineOnSceneGUI(targetCinemachine);
        }

        public static void CustomCinemachineOnSceneGUI(CustomCinemachine targetCinemachine)
        {
            for (int i = 0; i < targetCinemachine.cameraBatchList.Count; i++)
            {
                int index = i;
                ObjectBatchInfoInspectorEditor.ObjectBatchInfoOnSceneGUI(targetCinemachine.cameraBatchList[index], false);
            }
        }
    }
}
