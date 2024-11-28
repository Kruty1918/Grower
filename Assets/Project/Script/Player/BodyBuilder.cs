using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Grower;

public class BodyBuilder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform snakeHead; // Reference to the snake's head
    [SerializeField] private Cell bodyPrefab; // Prefab of the snake's body segment

    [Header("Settings")]
    [SerializeField] private float gridSize = 1f; // Size of the grid cells
    [SerializeField] private Vector3 spawnCellOffset; // Offset for segment spawning animation
    [SerializeField] private float animationDuration = 0.3f; // Duration for segment animation

    private Vector3 lastHeadGridPosition; // Last grid position of the head
    private List<Vector3> bodyPositions = new List<Vector3>(); // Stores positions of all body segments

    private void Start()
    {
        // Initialize with the head's starting position
        lastHeadGridPosition = SnapToGrid(snakeHead.position);
        bodyPositions.Add(lastHeadGridPosition);
    }

    private void FixedUpdate()
    {
        Vector3 currentHeadGridPosition = SnapToGrid(snakeHead.position);

        // Check if the head has moved to a new grid cell
        if (currentHeadGridPosition != lastHeadGridPosition)
        {
            // Add the last head position as the next body segment's position
            bodyPositions.Insert(0, lastHeadGridPosition);

            // Spawn a new body segment at the last position
            AnimateBodySegmentSpawn(lastHeadGridPosition);

            // Update the last head position
            lastHeadGridPosition = currentHeadGridPosition;

            // Limit the length of the body (if necessary)
            if (bodyPositions.Count > bodyPrefab.transform.childCount)
            {
                bodyPositions.RemoveAt(bodyPositions.Count - 1); // Remove the oldest position
            }
        }
    }

    private void AnimateBodySegmentSpawn(Vector3 targetPosition)
    {
        // Spawn the body segment at the head's position with an offset
        Cell segment = Instantiate(bodyPrefab, snakeHead.position + spawnCellOffset, Quaternion.identity);

        // Temporarily disable its collider
        Collider segmentCollider = segment.GetComponent<Collider>();
        if (segmentCollider != null)
        {
            segmentCollider.enabled = false;
        }

        // Move the segment to the target position using DOTween
        segment.transform.DOMove(targetPosition + spawnCellOffset, animationDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Delay activation of the collider by 1 second after animation ends
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    if (segmentCollider != null)
                    {
                        segmentCollider.enabled = true;
                        segment.PushCell();
                    }
                });
            });
    }


    private Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }
}
