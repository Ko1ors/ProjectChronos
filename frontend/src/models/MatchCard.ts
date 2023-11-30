import { type NFT } from '@thirdweb-dev/sdk'

export default interface MatchCard extends NFT {
  matchId: number
}
