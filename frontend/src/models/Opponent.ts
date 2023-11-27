import type UserDeck from './UserDeck.js'

export default interface Opponent {
  id: number
  name: string
  opponentDeck: UserDeck
}
