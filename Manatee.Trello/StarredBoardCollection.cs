﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of boards starred by the current member.
	/// </summary>
	public class StarredBoardCollection : ReadOnlyStarredBoardCollection, IStarredBoardCollection
	{
		internal StarredBoardCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth)
		{
		}

		/// <summary>
		/// Adds a board to the collection of starred boards.
		/// </summary>
		/// <param name="board">The board</param>
		/// <param name="position">The board's position in the starred boards list.</param>
		/// <param name="ct">(Optional) A cancellation token for async processing.</param>
		/// <returns>The <see cref="IStarredBoard"/> generated by Trello.</returns>
		public async Task<IStarredBoard> Add(IBoard board, Position position = null, CancellationToken ct = default)
		{
			var error = NotNullRule<IBoard>.Instance.Validate(null, board);
			if (error != null)
				throw new ValidationException<IBoard>(board, new[] {error});
			position = position ?? Position.Bottom;

			var json = TrelloConfiguration.JsonFactory.Create<IJsonStarredBoard>();
			json.Board = TrelloConfiguration.JsonFactory.Create<IJsonBoard>();
			json.Board.Id = board.Id;
			json.Pos = Position.GetJson(position);

			var endpoint = EndpointFactory.Build(EntityRequestType.Member_Write_AddStarredBoard, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = await JsonRepository.Execute(Auth, endpoint, json, ct);

			return new StarredBoard(OwnerId, newData, Auth);
		}
	}
}