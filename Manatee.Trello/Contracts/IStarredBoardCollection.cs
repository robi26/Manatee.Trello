﻿using System.Threading;
using System.Threading.Tasks;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of boards starred by the current member.
	/// </summary>
	public interface IStarredBoardCollection : IReadOnlyCollection<IStarredBoard>
	{
		/// <summary>
		/// Adds a board to the collection of starred boards.
		/// </summary>
		/// <param name="board">The board</param>
		/// <param name="position">The board's position in the starred boards list.</param>
		/// <param name="ct">(Optional) A cancellation token for async processing.</param>
		/// <returns>The <see cref="IStarredBoard"/> generated by Trello.</returns>
		Task<IStarredBoard> Add(IBoard board, Position position = null, CancellationToken ct = default);
	}
}