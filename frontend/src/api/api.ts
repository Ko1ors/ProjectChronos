import type UserDeck from '@/models/UserDeck'
import type DeckCard from '@/models/DeckCard'
import type { NFT } from '@thirdweb-dev/sdk'
import { PackType } from '../models/PackType'
import type Opponent from '../models/Opponent'
import type Match from '../models/Match'

export const apiUrl = import.meta.env.VITE_SERVICE_API_URL

export interface Response<T> {
  success: boolean
  message: string
  data: T | null
}

export const getOpponentsAsync = async function (): Promise<Response<Opponent[]>> {
  const response = getAsync<Opponent[]>(`${apiUrl}/GameSystem/GetOpponents`)
  return response
}

export const initiateMatchAsync = async function (opponentId: number): Promise<Response<Match>> {
  const response = postAsync<Match>(`${apiUrl}/GameSystem/InitiateMatch`, opponentId)
  return response
}

export const isAuthorizedAsync = async function (): Promise<Response<boolean>> {
  const response = getAsync<boolean>(`${apiUrl}/Users/UserAuthorized`)
  return response
}

export const isUserAdminAsync = async function (): Promise<Response<boolean>> {
  const response = getAsync<boolean>(`${apiUrl}/Users/UserAdmin`)
  return response
}

export const loginAsync = async function (
  address: string,
  signature: string,
  rememberMe: boolean
): Promise<Response<string>> {
  const response = postAsync<string>(`${apiUrl}/Users/Login`, { address, signature, rememberMe })
  return response
}

export const getActiveDeckAsync = async function (): Promise<Response<UserDeck | null>> {
  const response = getAsync<UserDeck | null>(`${apiUrl}/Decks/GetActiveDeck`)
  return response
}

export const getOwnedPacksAsync = async function (): Promise<Response<NFT[] | null>> {
  const response = getAsync<NFT[] | null>(`${apiUrl}/Packs/GetOwnedPacks`)
  return response
}

export const claimPackAsync = async function (packType: PackType): Promise<Response<boolean>> {
  const response = postAsync<boolean>(`${apiUrl}/Packs/ClaimPack`, { packType })
  return response
}

export const getPackContentAsync = async function (
  packType: PackType
): Promise<Response<NFT[] | null>> {
  const response = getAsync<NFT[] | null>(`${apiUrl}/Packs/GetPackContent?type=${packType}`)
  return response
}

export const createDeckAsync = async function (
  cards: DeckCard[],
  active: boolean
): Promise<Response<string>> {
  const response = postAsync<string>(`${apiUrl}/Decks/CreateCardDeck`, { cards, active })
  return response
}

export const deleteDeckAsync = async function (deckId: number): Promise<Response<string>> {
  const response = deleteAsync<string>(`${apiUrl}/Decks/DeleteCardDeck?deckId=${deckId}`, {
    deckId
  })
  return response
}

export const updateDeckAsync = async function (
  deckId: number,
  cards: DeckCard[],
  active: boolean
): Promise<Response<string>> {
  const response = putAsync<string>(`${apiUrl}/Decks/UpdateCardDeck`, { deckId, cards, active })
  return response
}

export const logoutAsync = async function (): Promise<Response<boolean>> {
  const response = postAsync<boolean>(`${apiUrl}/Users/Logout`, {})
  return response
}

export const getAuthMessagesAsync = async function (address: string): Promise<Response<string>> {
  const response = getAsync<string>(`${apiUrl}/Users/GetAuthMessage?address=${address}`)
  return response
}

const getContentAsync = async function (response: globalThis.Response) {
  const text = await response.text()
  try {
    return JSON.parse(text)
  } catch (e) {
    return text
  }
}

export const getAsync = async function <T>(url: string, options?: RequestInit) {
  const response = await fetch(url, {
    credentials: 'include',
    ...options
  })
  if (!response.ok) {
    const errorResponse: Response<T> = {
      success: false,
      message: await response.text(),
      data: null
    }
    console.error(errorResponse)
    return errorResponse
  }
  if (response.status == 204) {
    return { success: true, message: '', data: null }
  }
  return { success: true, message: '', data: (await getContentAsync(response)) as T }
}

export const postAsync = async function <T>(
  url: string,
  body: any,
  options?: RequestInit,
  isFile?: boolean
) {
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(body),
    credentials: 'include',
    ...options
  })
  if (!response.ok) {
    const errorResponse: Response<T> = {
      success: false,
      message: await response.text(),
      data: null
    }
    console.error(errorResponse)
    return errorResponse
  }
  return { success: true, message: '', data: (await getContentAsync(response)) as T }
}

export const putAsync = async function <T>(url: string, body: any, options?: RequestInit) {
  const response = await fetch(url, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(body),
    credentials: 'include',
    ...options
  })
  if (!response.ok) {
    const errorResponse: Response<T> = {
      success: false,
      message: await response.text(),
      data: null
    }
    console.error(errorResponse)
    return errorResponse
  }
  return { success: true, message: '', data: (await getContentAsync(response)) as T }
}

export const deleteAsync = async function <T>(url: string, body: any, options?: RequestInit) {
  const response = await fetch(url, {
    method: 'DELETE',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(body),
    credentials: 'include',
    ...options
  })
  if (!response.ok) {
    const errorResponse: Response<T> = {
      success: false,
      message: await response.text(),
      data: null
    }
    console.error(errorResponse)
    return errorResponse
  }
  return { success: true, message: '', data: (await getContentAsync(response)) as T }
}
