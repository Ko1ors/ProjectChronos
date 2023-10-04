import { ThirdwebSDK } from "@thirdweb-dev/sdk";
import getSdk from "./getSdk";

const getContractAsync = async (sdk: ThirdwebSDK | null = null) => {
    if(sdk === null) {
        sdk = getSdk();
    }
    const contract = await sdk.getContract(process.env.CONTRACT_ADDRESS!);
    return contract;
}

export default getContractAsync;