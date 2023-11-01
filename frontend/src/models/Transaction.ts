export default interface Transaction {
  tx: {
    erc20Rewards?:
      | {
          contractAddress: string
          quantityPerReward: string | number
        }[]
      | undefined
    erc721Rewards?:
      | {
          tokenId: (string | number | bigint | Number) &
            (string | number | bigint | Number | undefined)
          contractAddress: string
        }[]
      | undefined
    erc1155Rewards?:
      | {
          tokenId: (string | number | bigint | Number) &
            (string | number | bigint | Number | undefined)
          contractAddress: string
          quantityPerReward: (string | number | bigint | Number) &
            (string | number | bigint | Number | undefined)
        }[]
      | undefined
  }
}
