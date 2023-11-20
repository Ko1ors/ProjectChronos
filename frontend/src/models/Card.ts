import { type NFT } from '@thirdweb-dev/sdk'

export default interface Card extends NFT {
  list: number
}
