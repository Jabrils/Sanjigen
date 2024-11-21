import numpy as np

def check_winner(board):
    """Check if there's a winner in the Tic Tac Toe board."""
    winning_positions = [
        [board[0, 0], board[0, 1], board[0, 2]],
        [board[1, 0], board[1, 1], board[1, 2]],
        [board[2, 0], board[2, 1], board[2, 2]],
        [board[0, 0], board[1, 0], board[2, 0]],
        [board[0, 1], board[1, 1], board[2, 1]],
        [board[0, 2], board[1, 2], board[2, 2]],
        [board[0, 0], board[1, 1], board[2, 2]],
        [board[0, 2], board[1, 1], board[2, 0]]
    ]
    for line in winning_positions:
        if line[0] == line[1] == line[2] and line[0] != 0:
            return line[0]  # Return the winner (1 or 2)
    return 0  # Return 0 if there's no winner

def minimax(board, depth, is_maximizing, player):
    """
    Minimax algorithm to determine the best possible move.
    player = 1 for maximizing player, 2 for minimizing player.
    """
    opponent = 3 - player
    winner = check_winner(board)
    if winner == player:
        return 1
    elif winner == opponent:
        return -1
    elif np.all(board != 0):  # Board is full (draw)
        return 0

    if is_maximizing:
        best_score = -float('inf')
        for row in range(3):
            for col in range(3):
                if board[row, col] == 0:
                    board[row, col] = player
                    score = minimax(board, depth + 1, False, player)
                    board[row, col] = 0
                    best_score = max(score, best_score)
        return best_score
    else:
        best_score = float('inf')
        for row in range(3):
            for col in range(3):
                if board[row, col] == 0:
                    board[row, col] = opponent
                    score = minimax(board, depth + 1, True, player)
                    board[row, col] = 0
                    best_score = min(score, best_score)
        return best_score

def find_best_move(board, player):
    """
    Find the best move for the current player using the minimax algorithm.
    """
    best_score = -float('inf')
    best_move = None
    for row in range(3):
        for col in range(3):
            if board[row, col] == 0:
                board[row, col] = player
                score = minimax(board, 0, False, player)
                board[row, col] = 0
                if score > best_score:
                    best_score = score
                    best_move = (row, col)
    return best_move

def simulate_perfect_game():
    """Simulate a game where both players play perfectly."""
    board = np.zeros((3, 3), dtype=int)
    player_turn = 1  # Start with player 1
    
    while True:
        best_move = find_best_move(board, player_turn)
        if best_move is None:
            break  # Draw
        row, col = best_move
        board[row, col] = player_turn
        if check_winner(board):
            return player_turn  # Return the winner (1 or 2)
        if np.all(board != 0):
            return 0  # Draw
        player_turn = 3 - player_turn  # Alternate turns between 1 and 2
        print("Player Turn:", player_turn)

# Run multiple games
games = 10  # Change this number for more games()
results = [simulate_perfect_game() for _ in range(games)]
player1_wins = results.count(1)
player2_wins = results.count(2)
draws = results.count(0)

# Calculate win rates
player1_win_rate = player1_wins / games * 100
player2_win_rate = player2_wins / games * 100
draw_rate = draws / games * 100

print(f"Player 1 Win Rate: {player1_win_rate}%")
print(f"Player 2 Win Rate: {player2_win_rate}%")
print(f"Draw Rate: {draw_rate}%")
