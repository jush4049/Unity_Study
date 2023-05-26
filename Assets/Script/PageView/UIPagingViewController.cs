using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Animations;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(ScrollRect))]
    public class UIPagingViewController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        protected GameObject gbj_ContentRoot = null;

        [SerializeField]
        protected UIPageControl pageControl;

        [SerializeField]
        private float animationDuration = 0.3f;

        private float key1InTangent = 0f;
        private float key1OutTangent = 1f;
        private float key2InTangent = 1f;
        private float key2OutTangent = 0f;

        private bool isAnimating = false; // 애니메이션 재생 중임을 나타냄

        private Vector2 destPosition; // 최종 스크롤 위치
        private Vector2 initialPosition; // 자동 스크롤을 시작할 때의 스크롤 위치

        private AnimationCurve animationCurve; // 자동 스크롤 관련 애니메이션 커브

        private int prevPageIndex = 0; // 이전 페이지의 인덱스
        private Rect currentViewRect; // 스크롤 뷰의 사각형 크기

        public RectTransform CachedRectTransform
        {
            get
            {
                return GetComponent<RectTransform>();
            }
        }

        public ScrollRect CachedScrollRect
        {
            get
            {
                return GetComponent<ScrollRect>();
            }
        }

        // 드래그가 시작될 때 호출된다
        public void OnBeginDrag(PointerEventData eventData)
        {
            // 애니메이션 도중에 플래그를 리셋한다
            isAnimating = false;
        }
        // 드래그가 끝날 때 호출된다
        public void OnEndDrag(PointerEventData eventData)
        {
                GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();

                // 현재 동작 중인 스크롤 뷰를 멈춘다
                CachedScrollRect.StopMovement();

                // GridLayoutGroup의 cellSize와 spacing을 이용하여 한 페이지의 폭을 계산한다
                float pageWidth = -(grid.cellSize.x + grid.spacing.x);

                // 스크롤의 현재 위치로부터 맞출 페이지의 인덱스를  계산한다
                int pageIndex = Mathf.RoundToInt((CachedScrollRect.content.anchoredPosition.x) / pageWidth);

                if (pageIndex == prevPageIndex && Mathf.Abs(eventData.delta.x) >= 4)
                {
                    // 일정 속도 이상으로 드래그할 경우 해당 방향으로 한 페이지 진행시킨다.
                    CachedScrollRect.content.anchoredPosition += new Vector2(eventData.delta.x, 0.0f);
                    pageIndex += (int)Mathf.Sign(-eventData.delta.x);
                }

                // 첫 페이지 또는 끝 페이지일 경우에는 그 이상 스크롤하지 않도록 한다
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                else if (pageIndex > grid.transform.childCount - 1)
                {
                    pageIndex = grid.transform.childCount - 1;
                }

                prevPageIndex = pageIndex; // 현재 페이지의 인덱스를 유지한다

                // 최종적인 스크롤 위치를 계산한다
                float destX = pageIndex * pageWidth;
                destPosition = new Vector2(destX, CachedScrollRect.content.anchoredPosition.y);

                // 시작할 때의 스크롤 위치를 저장해둔다
                initialPosition = CachedScrollRect.content.anchoredPosition;

                // 애니메이션 커브를 작성한다.
                Keyframe keyFrame1 = new Keyframe(Time.time, 0.0f, key1InTangent, key1OutTangent);

                Keyframe keyframe2 = new Keyframe(Time.time + animationDuration, 1.0f, key2InTangent, key2OutTangent);
                animationCurve = new AnimationCurve(keyFrame1, keyframe2);

                // 애니메이션 재생 중임을 나타내는 플래그를 설정한다
                isAnimating = true;

                // 페이지 컨트롤 표시를 갱신한다
                if (pageControl != null)
                    pageControl.SetCurrentPage(pageIndex);
        }
            // 매 프레임 마다 Update 메서드가 처리된 다음에 호출된다
            void LateUpdate()
            {
                if (isAnimating)
                {
                    if (Time.time >= animationCurve.keys[animationCurve.length - 1].time)
                    {
                        // 애니메이션 커브의 마지막 키프레임을 지나가면 애니메이션을 끝낸다
                        CachedScrollRect.content.anchoredPosition = destPosition;
                        isAnimating = false;
                        return;
                    }

                    // 애니메이션 커브를 사용하여 현재 스크롤 위치를 계산하여 스크롤 뷰를 이동
                    Vector2 newPosition = initialPosition + (destPosition - initialPosition) * animationCurve.Evaluate(Time.time);
                    CachedScrollRect.content.anchoredPosition = newPosition;
                }
            }

            // 인스턴스를 로드할 때 Awake 메서드가 처리된 다음에 호출된다
            void Start()
            {
                UpdateView();

                if (pageControl != null)
                {
                    if(gbj_ContentRoot != null)
                        pageControl.SetNumberOfPages(gbj_ContentRoot.transform.childCount);
                    pageControl.SetCurrentPage(0); // 페이지 컨트롤 표시를 초기화한다
                }
            }

            void Update()
            {
                if (CachedRectTransform.rect.width != currentViewRect.width || CachedRectTransform.rect.height != currentViewRect.height)
                {
                    // 스크롤 뷰의 폭이나 높이가 변화하면 Scroll Content의 Padding을 갱신한다.
                    UpdateView();
                }
            }

            // Scroll Content의 Padding을 갱신하는 메서드
            private void UpdateView()
            {
                currentViewRect = CachedRectTransform.rect;

                // GridLayoutGroup의 cellSize를 사용하여 Scroll Content의 Padding을 계산하여 설정한다
                GridLayoutGroup grid = CachedScrollRect.content.GetComponent<GridLayoutGroup>();
                int paddingH = Mathf.RoundToInt((currentViewRect.width - grid.cellSize.x) / 2.0f);
                int paddingV = Mathf.RoundToInt((currentViewRect.height - grid.cellSize.y) / 2.0f);
                grid.padding = new RectOffset(paddingH, paddingH, paddingV, paddingV);
            }

    }
}

