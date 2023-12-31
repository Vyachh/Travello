import { ITrip } from "./ITrip"

export interface IUserInfo {
    id: string
    userName: string
    currentTripId: number
    tripList: ITrip[]
    role: string
    email: string
    birthdate: string
    image: string
    isSelected: boolean
}