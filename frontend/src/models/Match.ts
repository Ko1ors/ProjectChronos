export default interface Match {
  id: number
  userId: string
  opponentId: number
  result: number
  systemVersion: number
  createdAt: string
  turns: Turn[]
}

interface Turn {
  cards?: Card[]
  attackCardId?: number
  targetCardId?: number
  isEvaded?: boolean
  attackDamage?: number
  targetHealth?: number
  index: number
  isUserTurn: boolean
  turnType: number
}

interface Card {
  id: number
  cardId: number
}
