export default interface Transaction {
  erc20Rewards?: {
    contractAddress: string
    quantityPerReward: string | number
  }[]
  erc721Rewards?: {
    tokenId: string | number | bigint | undefined
    contractAddress: string
  }[]
  erc1155Rewards?: {
    tokenId: (string | number | bigint) & (string | number | bigint | undefined)
    contractAddress: string
    quantityPerReward: (string | number | bigint) & (string | number | bigint | undefined)
  }[] // Use a more permissive type here
}
