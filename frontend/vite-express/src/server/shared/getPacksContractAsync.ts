import { ThirdwebSDK } from "@thirdweb-dev/sdk";
import getSdk from "./getSdk";

const getPacksContractAsync = async (sdk: ThirdwebSDK | null = null) => {
    if(sdk === null) {
        sdk = getSdk();
    }
    const contract = await sdk.getContract(process.env.PACK_CONTRACT_ADDRESS!, "pack");
    return contract;
}

export default getPacksContractAsync;