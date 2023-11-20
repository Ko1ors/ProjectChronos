import type DeckCard from '../models/DeckCard.ts'

export default interface UserDeck {
  id: number
  name: string
  active: boolean
  cards: DeckCard[]
}
