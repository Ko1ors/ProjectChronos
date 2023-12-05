export default interface UserInfo {
  userId: string
  userAddress: string
  totalOwnedCards: number
  totalUniqueOwnedCards: number
  totalOwnedCardsByElement: OwnedCardsInfo
  totalOwnedCardsByRarity: OwnedCardsInfo
  totalOwnedCardsByClass: OwnedCardsInfo
  wins: number
  losses: number
  draws: number
  totalMatches: number
}

interface OwnedCardsInfo {
  groupName: string
  items: {
    name: string
    value: number
  }[]
}
